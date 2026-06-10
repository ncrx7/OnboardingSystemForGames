using System;

namespace Onboarding
{
    public interface IOnboardingLevelProvider
    {
        int CurrentLevelIndex { get; }
        event Action OnLevelReady;
    }
}
