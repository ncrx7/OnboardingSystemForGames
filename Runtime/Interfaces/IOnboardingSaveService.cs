namespace Onboarding
{
    public interface IOnboardingSaveService
    {
        OnboardingData Load();
        void Save(OnboardingData data);
    }
}
