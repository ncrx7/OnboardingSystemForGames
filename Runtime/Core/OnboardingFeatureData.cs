using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Onboarding
{
    [MovedFrom(true, sourceNamespace: "CoreGameplay.Core.Onboarding")]
    [CreateAssetMenu(fileName = "NewOnboardingFeature", menuName = "Onboarding/Onboarding Feature Data")]
    public class OnboardingFeatureData : SerializedScriptableObject
    {
        public string FeatureId;
        public string FeatureName;
        public Sprite FeatureSprite;
        [TextArea] public string FeatureDescription;
        public int LevelId;
        public bool HasActionAfterClaim;
        [ShowIf(nameof(HasActionAfterClaim))]
        public List<IOnboardingAction> ActionsAfterClaim = new();
    }
}
