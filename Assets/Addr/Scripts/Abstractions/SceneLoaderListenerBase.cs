using System.Collections;
using Studio.OverOne.Addr.Events.Data;
using UnityEngine;

namespace Studio.OverOne.Addr.Abstractions
{
    internal abstract class SceneLoaderListenerBase : MonoBehaviour, ISceneLoaderListener
    {
        public virtual void AfterLoad(AfterLoadData data) { }

        public virtual IEnumerator AfterLoadAsync(AfterLoadData data) { yield break; }

        public virtual void AfterLoadingScreen(AfterLoadingScreenData data) { }
        
        public virtual IEnumerator AfterLoadingScreenAsync(AfterLoadingScreenData data) { yield break; }

        public virtual void BeforeLoad(BeforeLoadData data) { }
        
        public virtual IEnumerator BeforeLoadAsync(BeforeLoadData data) { yield break; }

        public virtual void BeforeLoadingScreen(BeforeLoadingScreenData data) { }
        
        public virtual IEnumerator BeforeLoadingScreenAsync(BeforeLoadingScreenData data) { yield break; }

        public virtual void OnLoadEnd(OnLoadEndData data) { }
        
        public virtual IEnumerator OnLoadEndAsync(OnLoadEndData data) { yield break; }

        public virtual void OnLoadingTick(OnLoadingTickData data) { }
        
        public virtual IEnumerator OnLoadingTickAsync(OnLoadingTickData data) { yield break; }

        public virtual void OnLoadStart(OnLoadStartData data) { }
        
        public virtual IEnumerator OnLoadStartAsync(OnLoadStartData data) { yield break; }

        public virtual void OnUnloadEnd(OnUnloadEndData data) { }
        
        public virtual IEnumerator OnUnloadEndAsync(OnUnloadEndData data) { yield break; }

        public virtual void OnUnloadingTick(OnUnloadingTickData data) { }
        
        public virtual IEnumerator OnUnloadingTickAsync(OnUnloadingTickData data) { yield break; }

        public virtual void OnUnloadStart(OnUnloadStartData data) { }
        
        public virtual IEnumerator OnUnloadStartAsync(OnUnloadStartData data) { yield break; }
    }
}