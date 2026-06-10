using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace Onboarding
{
    public class OnboardingManager : IOnboardingManager, IInitializable, IDisposable
    {
        private const string OnboardingLabel = "Onboarding";

        private readonly IOnboardingLevelProvider _levelProvider;
        private readonly IOnboardingSaveService _saveService;
        private readonly NewFeaturePopUp _popUp;
        private readonly DiContainer _container;
        private readonly OnboardingData _data;
        private readonly Dictionary<int, List<OnboardingFeatureData>> _featuresByLevel = new();
        private readonly Queue<OnboardingFeatureData> _pendingFeatures = new();
        private readonly HashSet<string> _activeFeatureIds = new();
        private readonly List<AsyncOperationHandle> _handles = new();

        private bool _featuresLoaded;
        private bool _isShowingPopUp;
        private bool _isOnboardingPending;
        private bool _isPopUpPhase;
        public bool IsOnboardingActive => _isOnboardingPending || _isShowingPopUp;
        public bool IsOnboardingPreAction => _isOnboardingPending || _isPopUpPhase;

        private event Action<string> OnSignalRaised;

        public OnboardingManager(IOnboardingLevelProvider levelProvider, IOnboardingSaveService saveService, NewFeaturePopUp popUp, DiContainer container)
        {
            _levelProvider = levelProvider;
            _saveService = saveService;
            _popUp = popUp;
            _container = container;
            _data = _saveService.Load();
        }

        public bool IsFeatureClaimed(string featureId) => _data.claimedFeatureIds.Contains(featureId);

        public void RaiseSignal(string signalId)
        {
            OnSignalRaised?.Invoke(signalId);
        }

        public UniTask WaitForSignalAsync(string signalId)
        {
            var completionSource = new UniTaskCompletionSource();

            void Handler(string raisedSignalId)
            {
                if (raisedSignalId != signalId) return;
                OnSignalRaised -= Handler;
                completionSource.TrySetResult();
            }

            OnSignalRaised += Handler;
            return completionSource.Task;
        }

        public void Initialize()
        {
            _levelProvider.OnLevelReady += HandleLevelReady;
            _popUp.OnClaimed += HandleFeatureClaimed;

            Debug.Log("[OnboardingManager] Manager inited");
        }

        public void Dispose()
        {
            _levelProvider.OnLevelReady -= HandleLevelReady;
            _popUp.OnClaimed -= HandleFeatureClaimed;

            foreach (var handle in _handles)
                if (handle.IsValid()) Addressables.Release(handle);

            _handles.Clear();
            _featuresByLevel.Clear();
            _pendingFeatures.Clear();
            _activeFeatureIds.Clear();
        }

        private void HandleLevelReady()
        {
            Debug.Log("[OnboardingManager] Handle level ready");
            _isOnboardingPending = true;
            HandleLevelReadyAsync().Forget();
        }

        private async UniTaskVoid HandleLevelReadyAsync()
        {
            if (!_featuresLoaded)
                await LoadFeaturesAsync();

            if (!_featuresByLevel.TryGetValue(_levelProvider.CurrentLevelIndex, out var features))
            {
                _isOnboardingPending = false;
                return;
            }

            foreach (var feature in _featuresByLevel)
            {
                foreach (var item in feature.Value)
                {
                    Debug.Log("[OnboardingManager] loaded new feature so: " + item.name);
                }
            }

            foreach (var feature in features)
            {
                if (_data.claimedFeatureIds.Contains(feature.FeatureId))
                    continue;

                if (!_activeFeatureIds.Add(feature.FeatureId))
                    continue;

                _pendingFeatures.Enqueue(feature);
            }

            _isOnboardingPending = false;
            TryShowNextPopUp();
        }

        private void TryShowNextPopUp()
        {
            if (_isShowingPopUp || _pendingFeatures.Count == 0)
                return;

            _isShowingPopUp = true;
            _isPopUpPhase = true;
            _popUp.Show(_pendingFeatures.Dequeue());
        }

        private void HandleFeatureClaimed(OnboardingFeatureData feature)
        {
            HandleFeatureClaimedAsync(feature).Forget();
        }

        private async UniTaskVoid HandleFeatureClaimedAsync(OnboardingFeatureData feature)
        {
            _isPopUpPhase = false;

            if (_data.claimedFeatureIds.Add(feature.FeatureId))
                _saveService.Save(_data);

            _activeFeatureIds.Remove(feature.FeatureId);

            if (feature.HasActionAfterClaim)
            {
                foreach (var action in feature.ActionsAfterClaim)
                {
                    if (action != null)
                        await action.ExecuteAsync(_container);
                }
            }

            _isShowingPopUp = false;
            TryShowNextPopUp();
        }

        private async UniTask LoadFeaturesAsync()
        {
            _featuresLoaded = true;

            var locationsHandle = Addressables.LoadResourceLocationsAsync(OnboardingLabel, typeof(OnboardingFeatureData));
            await locationsHandle.Task;

            if (locationsHandle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"[OnboardingManager] Failed to query addressables with label '{OnboardingLabel}'.");
                Addressables.Release(locationsHandle);
                return;
            }

            foreach (var location in locationsHandle.Result)
            {
                var handle = Addressables.LoadAssetAsync<OnboardingFeatureData>(location);
                _handles.Add(handle);
                await handle.Task;

                if (handle.Status != AsyncOperationStatus.Succeeded || handle.Result == null)
                {
                    Debug.LogError($"[OnboardingManager] Failed to load addressable '{location.PrimaryKey}'.");
                    continue;
                }

                var feature = handle.Result;
                if (!_featuresByLevel.TryGetValue(feature.LevelId, out var levelFeatures))
                {
                    levelFeatures = new List<OnboardingFeatureData>();
                    _featuresByLevel[feature.LevelId] = levelFeatures;
                }
                levelFeatures.Add(feature);
            }

            Addressables.Release(locationsHandle);
        }
    }
}
