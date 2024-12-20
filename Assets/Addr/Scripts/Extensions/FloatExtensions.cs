using System.Collections;
using UnityEngine;

namespace Studio.OverOne.Addr.Extensions
{
    internal static class FloatExtensions
    {
        public static IEnumerator Lerp(this float value, float to, float duration, System.Action<float> onUpdate)
        {
            var lCurrentTime = 0f;
            var lTotalTime = 0f;
            while (lCurrentTime <= 1f)
            {
                var lValue = Mathf.Lerp(value, to, lCurrentTime);
                onUpdate?.Invoke(lValue);
                
                lTotalTime += Time.deltaTime;
                lCurrentTime = lTotalTime / duration;
                yield return null;
            }
            
            onUpdate?.Invoke(to);
        }
    }
}