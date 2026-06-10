using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Onboarding
{
    public interface IUnmaskOverlayPresenter
    {
        bool IsPresenting { get; }
        public UniTask PresentAsync(RectTransform target, float scaleMultiplier);
        public UniTask PresentWorldAsync(Vector3 worldPosition, Vector3 worldSize, float scaleMultiplier);
        public UniTask DismissAsync();
    }
}
