using Cysharp.Threading.Tasks;
using Zenject;

namespace Onboarding
{
    public interface IOnboardingDismissCondition
    {
        public UniTask WaitAsync(DiContainer container);
    }
}
