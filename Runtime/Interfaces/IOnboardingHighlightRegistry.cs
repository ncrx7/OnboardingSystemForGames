using UnityEngine;

namespace Onboarding
{
    public interface IOnboardingHighlightRegistry
    {
        public void Register(string anchorId, RectTransform rectTransform);
        public void Unregister(string anchorId);
        public bool TryGetAnchor(string anchorId, out RectTransform rectTransform);
    }
}
