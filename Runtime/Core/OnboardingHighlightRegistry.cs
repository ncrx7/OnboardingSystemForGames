using System.Collections.Generic;
using UnityEngine;

namespace Onboarding
{
    public class OnboardingHighlightRegistry : IOnboardingHighlightRegistry
    {
        private readonly Dictionary<string, RectTransform> _anchors = new();

        public void Register(string anchorId, RectTransform rectTransform)
        {
            _anchors[anchorId] = rectTransform;
        }

        public void Unregister(string anchorId)
        {
            _anchors.Remove(anchorId);
        }

        public bool TryGetAnchor(string anchorId, out RectTransform rectTransform)
        {
            return _anchors.TryGetValue(anchorId, out rectTransform);
        }
    }
}
