using Studio.OverOne.Addr.Abstractions;
using Studio.OverOne.Addr.Events.Data.Abstractions;

namespace Studio.OverOne.Addr.Events.Data
{
    internal partial class AfterLoadingScreenData : EventBase
    {
        public AfterLoadingScreenData(ISceneLoader sceneLoader, float startTime, float endTime = -1f)
            : base(sceneLoader, startTime, endTime)
        {
            // empty
        }
    }
}