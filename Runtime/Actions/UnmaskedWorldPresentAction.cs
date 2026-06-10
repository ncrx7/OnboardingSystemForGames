using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using Zenject;

namespace Onboarding
{
    [MovedFrom(true, sourceNamespace: "CoreGameplay.Core.Onboarding")]
    public class UnmaskedWorldPresentAction : IOnboardingAction
    {
        public Vector3 Position;
        public Vector3 WorldSize = Vector3.one;
        [Range(1f, 2f)] public float ScaleMultiplier = 1.15f;
        public IOnboardingDismissCondition DismissCondition;

        public async UniTask ExecuteAsync(DiContainer container)
        {
            UniTask dismissTask = DismissCondition != null
                ? DismissCondition.WaitAsync(container)
                : UniTask.CompletedTask;

            var presenter = container.Resolve<IUnmaskOverlayPresenter>();
            await presenter.PresentWorldAsync(Position, WorldSize, ScaleMultiplier);
            await dismissTask;
            await presenter.DismissAsync();
        }
    }
}
