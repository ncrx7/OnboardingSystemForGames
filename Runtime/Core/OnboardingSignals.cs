namespace Onboarding
{
    public static class OnboardingSignals
    {
        public static string FeatureClaimed(string featureId) => "onboarding_feature_claimed_" + featureId;
    }
}
