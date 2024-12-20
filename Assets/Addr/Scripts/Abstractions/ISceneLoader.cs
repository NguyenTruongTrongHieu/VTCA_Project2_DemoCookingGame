using System.Collections;

namespace Studio.OverOne.Addr.Abstractions
{
    internal interface ISceneLoader
    {
        bool CanExecuteLoad { get; }

        bool IsLoaded { get; }

        bool IsLoading { get; }

        bool IsUnloaded { get; }

        bool IsUnloading { get; }
        
        float Progress { get; }

        int SceneCount { get; }

        void DontDestroy();
        
        void Load();

        IEnumerator LoadAsync();

        void Unload();
    }
}