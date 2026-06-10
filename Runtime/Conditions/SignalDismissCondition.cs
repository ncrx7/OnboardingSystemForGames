using Cysharp.Threading.Tasks;
using UnityEngine.Scripting.APIUpdating;
using Zenject;

namespace Onboarding
{
    [MovedFrom(true, sourceNamespace: "CoreGameplay.Core.Onboarding")]
    public class SignalDismissCondition : IOnboardingDismissCondition
    {
        public string SignalId;

        public UniTask WaitAsync(DiContainer container)
        {
            return container.Resolve<IOnboardingManager>().WaitForSignalAsync(SignalId);
        }
    }
}
