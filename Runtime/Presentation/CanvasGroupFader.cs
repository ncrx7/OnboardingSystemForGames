using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Onboarding
{
    public static class CanvasGroupFader
    {
        public static async UniTask FadeAsync(CanvasGroup canvasGroup, float targetAlpha, float duration, bool easeOut)
        {
            float startAlpha = canvasGroup.alpha;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                t = easeOut ? 1f - (1f - t) * (1f - t) * (1f - t) : t * t * t;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
                await UniTask.Yield();
            }

            canvasGroup.alpha = targetAlpha;
        }
    }
}
