using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

namespace Onboarding
{
    [MovedFrom(true, sourceNamespace: "CoreGameplay.Core.Onboarding")]
    [RequireComponent(typeof(CanvasGroup))]
    public class NewFeaturePopUp : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _featureIcon;
        [SerializeField] private TMP_Text _featureNameText;
        [SerializeField] private TMP_Text _featureDescriptionText;
        [SerializeField] private Button _claimButton;
        [SerializeField] private float _showDuration = 0.35f;
        [SerializeField] private float _hideDuration = 0.25f;

        public event Action<OnboardingFeatureData> OnClaimed;

        private OnboardingFeatureData _currentFeature;

        private void Awake()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            _claimButton.onClick.AddListener(HandleClaimClicked);
        }

        private void OnDestroy()
        {
            _claimButton.onClick.RemoveListener(HandleClaimClicked);
        }

        public void Show(OnboardingFeatureData feature)
        {
            _currentFeature = feature;
            _featureIcon.sprite = feature.FeatureSprite;
            _featureNameText.text = feature.FeatureName;
            _featureDescriptionText.text = feature.FeatureDescription;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            CanvasGroupFader.FadeAsync(_canvasGroup, 1f, _showDuration, easeOut: true).Forget();
        }

        private UniTask HideAsync()
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            return CanvasGroupFader.FadeAsync(_canvasGroup, 0f, _hideDuration, easeOut: false);
        }

        private void HandleClaimClicked()
        {
            HandleClaimClickedAsync().Forget();
        }

        private async UniTaskVoid HandleClaimClickedAsync()
        {
            var feature = _currentFeature;
            _currentFeature = null;
            await HideAsync();
            OnClaimed?.Invoke(feature);
        }
    }
}
