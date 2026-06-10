using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using Zenject;

namespace Onboarding
{
    [MovedFrom(true, sourceNamespace: "CoreGameplay.Core.Onboarding")]
    public class UnmaskedPresentAction : IOnboardingAction
    {
        public string TargetAnchorId;
        [Range(1f, 2f)] public float ScaleMultiplier = 1.15f;
        public IOnboardingDismissCondition DismissCondition;

        public async UniTask ExecuteAsync(DiContainer container)
        {
            var registry = container.Resolve<IOnboardingHighlightRegistry>();
            if (!registry.TryGetAnchor(TargetAnchorId, out var target))
            {
                Debug.LogError($"[UnmaskedPresentAction] Highlight anchor not found: '{TargetAnchorId}'.");
                return;
            }

            UniTask dismissTask = DismissCondition != null
                ? DismissCondition.WaitAsync(container)
                : UniTask.CompletedTask;

            var presenter = container.Resolve<IUnmaskOverlayPresenter>();
            await presenter.PresentAsync(target, ScaleMultiplier);
            await dismissTask;
            await presenter.DismissAsync();
        }
    }
}
