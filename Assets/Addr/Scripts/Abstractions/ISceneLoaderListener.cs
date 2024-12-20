using System.Collections;
using Studio.OverOne.Addr.Events.Data;

namespace Studio.OverOne.Addr.Abstractions
{
    internal interface ISceneLoaderListener
    {
        void AfterLoad(AfterLoadData data);
        
        IEnumerator AfterLoadAsync(AfterLoadData data);
        
        void AfterLoadingScreen(AfterLoadingScreenData data);

        IEnumerator AfterLoadingScreenAsync(AfterLoadingScreenData data);
        
        void BeforeLoad(BeforeLoadData data);
        
        IEnumerator BeforeLoadAsync(BeforeLoadData data);
        
        void BeforeLoadingScreen(BeforeLoadingScreenData data);
        
        IEnumerator BeforeLoadingScreenAsync(BeforeLoadingScreenData data);
        
        void OnLoadEnd(OnLoadEndData data);
        
        IEnumerator OnLoadEndAsync(OnLoadEndData data);

        void OnLoadingTick(OnLoadingTickData data);
        
        IEnumerator OnLoadingTickAsync(OnLoadingTickData data);
        
        void OnLoadStart(OnLoadStartData data);
        
        IEnumerator OnLoadStartAsync(OnLoadStartData data);

        void OnUnloadEnd(OnUnloadEndData data);
        
        IEnumerator OnUnloadEndAsync(OnUnloadEndData data);

        void OnUnloadingTick(OnUnloadingTickData data);
        
        IEnumerator OnUnloadingTickAsync(OnUnloadingTickData data);

        void OnUnloadStart(OnUnloadStartData data);
        
        IEnumerator OnUnloadStartAsync(OnUnloadStartData data);
    }
}