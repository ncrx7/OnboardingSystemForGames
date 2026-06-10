using Cysharp.Threading.Tasks;

namespace Onboarding
{
    public interface IOnboardingManager
    {
        bool IsOnboardingActive { get; }
        bool IsOnboardingPreAction { get; }
        public bool IsFeatureClaimed(string featureId);
        public void RaiseSignal(string signalId);
        public UniTask WaitForSignalAsync(string signalId);
    }
}
