using System.Collections;

namespace Studio.OverOne.Addr.Abstractions
{
    internal interface ILoadingTracker
    {
        IEnumerator Close();

        IEnumerator Open(float fadeDuration, ISceneLoader loader, Optional<int> sortOrder);
    }
}