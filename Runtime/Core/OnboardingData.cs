using System;
using System.Collections.Generic;

namespace Onboarding
{
    [Serializable]
    public class OnboardingData
    {
        public HashSet<string> claimedFeatureIds = new HashSet<string>();
    }
}
