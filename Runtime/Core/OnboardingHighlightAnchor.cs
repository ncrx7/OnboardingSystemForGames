using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using Zenject;

namespace Onboarding
{
    [MovedFrom(true, sourceNamespace: "CoreGameplay.Core.Onboarding")]
    public class OnboardingHighlightAnchor : MonoBehaviour
    {
        [SerializeField] private string _anchorId;
        [SerializeField] private RectTransform _rectTransform;

        private IOnboardingHighlightRegistry _registry;

        [Inject]
        private void Construct(IOnboardingHighlightRegistry registry)
        {
            _registry = registry;
        }

        private void Reset()
        {
            _rectTransform = transform as RectTransform;
        }

        private void OnEnable()
        {
            _registry?.Register(_anchorId, _rectTransform);
        }

        private void OnDisable()
        {
            _registry?.Unregister(_anchorId);
        }
    }
}
