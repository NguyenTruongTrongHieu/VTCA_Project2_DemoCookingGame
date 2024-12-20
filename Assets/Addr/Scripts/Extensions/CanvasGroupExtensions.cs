using System.Collections;
using UnityEngine;

namespace Studio.OverOne.Addr.Extensions
{
    internal static class CanvasGroupExtensions
    {
        public static IEnumerator Fade(this CanvasGroup canvasGroup, float to, float duration,
            bool blockRaycasts = true)
        {
            if (canvasGroup == null || !canvasGroup.gameObject.activeInHierarchy)
                yield break;

            canvasGroup.blocksRaycasts = blockRaycasts;

            if (canvasGroup.alpha == to)
                yield break;

            yield return canvasGroup.alpha.Lerp(to, duration, f =>
            {
                canvasGroup.alpha = f;

                if (blockRaycasts)
                    canvasGroup.blocksRaycasts = canvasGroup.alpha > 0;
            });
        }
    }
}