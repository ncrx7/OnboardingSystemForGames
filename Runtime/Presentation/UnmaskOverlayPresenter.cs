using Coffee.UIExtensions;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Onboarding
{
    [MovedFrom(true, sourceNamespace: "CoreGameplay.Core.Onboarding")]
    [RequireComponent(typeof(CanvasGroup))]
    public class UnmaskOverlayPresenter : MonoBehaviour, IUnmaskOverlayPresenter
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Unmask _unmask;
        [SerializeField] private RectTransform _worldProxyRect;
        [SerializeField] private float _fadeDuration = 0.3f;
        [SerializeField] private Canvas _canvas;

        private bool _isPresenting;
        public bool IsPresenting => _isPresenting;

        private void Awake()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        public UniTask PresentAsync(RectTransform target, float scaleMultiplier)
        {
            _unmask.FitTo(target);
            return FadeInAsync(scaleMultiplier);
        }

        public UniTask PresentWorldAsync(Vector3 worldPosition, Vector3 worldSize, float scaleMultiplier)
        {
            FitProxyToWorldPosition(worldPosition, worldSize);
            _unmask.FitTo(_worldProxyRect);
            return FadeInAsync(scaleMultiplier);
        }

        private UniTask FadeInAsync(float scaleMultiplier)
        {
            var unmaskRect = (RectTransform)_unmask.transform;
            unmaskRect.localScale *= scaleMultiplier;
            _isPresenting = true;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            return FadeAsync(1f);
        }

        private void FitProxyToWorldPosition(Vector3 worldPosition, Vector3 worldSize)
        {
            var canvasRect = _canvas.transform as RectTransform;
            var canvasCamera = _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera;

            Camera worldCamera = Camera.main;

            Vector3 halfSize = worldSize * 0.5f;

            Vector2 screenMin = worldCamera.WorldToScreenPoint(worldPosition - halfSize);
            Vector2 screenMax = worldCamera.WorldToScreenPoint(worldPosition + halfSize);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenMin, canvasCamera, out var localMin);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenMax, canvasCamera, out var localMax);

            _worldProxyRect.anchoredPosition = (localMin + localMax) * 0.5f;
            _worldProxyRect.sizeDelta = new Vector2(Mathf.Abs(localMax.x - localMin.x), Mathf.Abs(localMax.y - localMin.y));
        }

        public async UniTask DismissAsync()
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            await FadeAsync(0f);
            _isPresenting = false;
        }

        private UniTask FadeAsync(float targetAlpha)
        {
            return CanvasGroupFader.FadeAsync(_canvasGroup, targetAlpha, _fadeDuration, easeOut: true);
        }
    }
}
