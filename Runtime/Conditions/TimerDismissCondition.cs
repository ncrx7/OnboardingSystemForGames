using System;
using Cysharp.Threading.Tasks;
using UnityEngine.Scripting.APIUpdating;
using Zenject;

namespace Onboarding
{
    [MovedFrom(true, sourceNamespace: "CoreGameplay.Core.Onboarding")]
    public class TimerDismissCondition : IOnboardingDismissCondition
    {
        public float Seconds = 2f;

        public UniTask WaitAsync(DiContainer container)
        {
            return UniTask.Delay(TimeSpan.FromSeconds(Seconds));
        }
    }
}
