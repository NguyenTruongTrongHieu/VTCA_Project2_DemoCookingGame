using Studio.OverOne.Addr.Abstractions;
using Studio.OverOne.Addr.Events.Data.Abstractions;

namespace Studio.OverOne.Addr.Events.Data
{
    internal partial class OnUnloadStartData : EventBase
    {
        public OnUnloadStartData(ISceneLoader sceneLoader, float startTime, float endTime = -1f)
            : base(sceneLoader, startTime, endTime)
        {
            // empty
        }
    }
}