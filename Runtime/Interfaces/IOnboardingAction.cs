using Cysharp.Threading.Tasks;
using Zenject;

namespace Onboarding
{
    public interface IOnboardingAction
    {
        public UniTask ExecuteAsync(DiContainer container);
    }
}
