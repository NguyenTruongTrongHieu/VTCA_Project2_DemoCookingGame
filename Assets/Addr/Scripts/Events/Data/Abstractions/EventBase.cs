using Studio.OverOne.Addr.Abstractions;

namespace Studio.OverOne.Addr.Events.Data.Abstractions
{
    internal abstract class EventBase
    {
        /// <summary>
        /// The timestamp, from Time.time when Addr finsihed loading or unloading
        /// </summary>
        public float EndTime { get; }
        
        /// <summary>
        /// Reference to the <see cref="SceneLoader"/> that triggered this event
        /// </summary>
        public ISceneLoader SceneLoader { get; }
        
        /// <summary>
        /// The timestamp, from Time.time when Addr started loading or unloading
        /// </summary>
        public float StartTime { get; }
        
        /// <summary>
        /// The amount of time, in seconds Addr has been loading or unloading
        /// </summary>
        public float Time { get; set; }

        protected EventBase(ISceneLoader sceneLoader, float startTime, float endTime = -1f)
        {
            EndTime = endTime;
            SceneLoader = sceneLoader;
            StartTime = startTime;
        }
    }
}