using Studio.OverOne.Addr.Events.Data;
using UnityEngine.Events;

namespace Studio.OverOne.Addr.Events
{
    [System.Serializable]
    internal sealed class AfterLoadEvent : UnityEvent<AfterLoadData>
    {
        // empty
    }
}