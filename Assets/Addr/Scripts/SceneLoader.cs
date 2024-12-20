using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Studio.OverOne.Addr.Abstractions;
using Studio.OverOne.Addr.Events;
using Studio.OverOne.Addr.Events.Data;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Studio.OverOne.Addr
{
    internal partial class SceneLoader : SceneLoaderListenerBase, ISceneLoader
    {
        public static event System.Action<BeforeLoadData> beforeLoad = delegate { };

        public static event System.Action<BeforeLoadingScreenData> beforeLoadingScreen = delegate { };

        public static event System.Action<OnLoadStartData> onLoadStart = delegate { };

        public static event System.Action<OnLoadingTickData> onLoadingTick = delegate { };

        public static event System.Action<OnLoadEndData> onLoadEnd = delegate { };

        public static event System.Action<OnUnloadStartData> onUnloadStart = delegate { };

        public static event System.Action<OnUnloadingTickData> onUnloadingTick = delegate { };

        public static event System.Action<OnUnloadEndData> onUnloadEnd = delegate { };

        public static event System.Action<AfterLoadData> afterLoad = delegate { };

        public static event System.Action<AfterLoadingScreenData> afterLoadingScreen = delegate { };

        public string AssetGuid => _scenes.FirstOrDefault()?.AssetGUID;
        
        public bool CanExecuteLoad => IsLoading == false && IsUnloading == false && IsUnloaded;
        
        public bool CanExecuteUnload => IsLoading == false && IsUnloading == false && IsLoaded;
        
        public bool IsLoaded { get; private set; }
        
        public bool IsLoading { get; private set; }

        public bool IsUnloaded { get; private set; } = true;
        
        public bool IsUnloading { get; private set; }
        
        public float Progress
        {
            get
            {
                var lCurrentProgress = _sceneHandles.Sum(x => x.PercentComplete);
                return (lCurrentProgress / SceneCount) * 100f;
            }
        }

        public int SceneCount => _scenes.Count;
        
#region " Inspector Variables "
        [Header("Behaviour Configuration")]
        [Tooltip("Indicates whether this loader should be promoted to DontDestroyOnLoad")]
        [SerializeField] private bool _dontDestroyOnLoad = true;
        
        [Tooltip("Indicates whether loading should begin in OnStart")]
        [SerializeField] private bool _loadOnStart;
        
        [Tooltip("Indicates whether the first scene within the scenes collection will be set to the active scene")]
        [SerializeField] private bool _setFirstSceneAsActive = true;

        [Space(10)]
        [Tooltip("Indicates whether loading should unload previously loaded SceneLoaders")]
        [SerializeField] private bool _unloadPrevious = true;
                
        [Tooltip("Indicates whether unloading this SceneLoader should cause its GameObject to be deleted")]
        [SerializeField] private bool _destroyOnUnload = true;

        [Header("Loading Configuration")]
        [Tooltip("Sort Order to set the Loading Scene Canvas to")]
        [SerializeField] private Optional<int> _loadingSceneSortOrder = new Optional<int>(999);
        
        [Tooltip("Amount of time, in seconds it will take to load the LoadingScene")]
        [SerializeField] private Optional<float> _loadingFadeDuration = new Optional<float>(1f);
                
        [Tooltip("Amount of time, in seconds the LoadingScene will remain visible")]
        [SerializeField] private Optional<float> _minimumLoadingTime = new Optional<float>(1f);
        
        [Tooltip("Reference to the LoadingScene AssetReference")]
        [SerializeField] private Optional<AssetReference> _loadingScene = new Optional<AssetReference>(null);
                
        [Space(10)]
        [Tooltip("References to the Scene AssetReferences that will be loaded. The first scene in the collection will be set as Active")]
        [SerializeField] private List<AssetReference> _scenes = new List<AssetReference>();

        [Header("Debug Configuration")]
        [Tooltip("Indicates whether events should be logged to the console")]
        [SerializeField] private bool _loggerEnabled;                

        [Header("Loading Events")]
        [Tooltip("Event triggered before loading is started")]
        [SerializeField] private BeforeLoadEvent _beforeLoad = new BeforeLoadEvent();

        [Tooltip("Event triggered before showing the loading screen")]
        [SerializeField] private BeforeLoadingScreenEvent _beforeLoadingScreen = new BeforeLoadingScreenEvent();
        
        [Tooltip("Event triggered when loading is started")]
        [SerializeField] private OnLoadStartEvent _onLoadStart = new OnLoadStartEvent();
                
        [Tooltip("Event triggered each frame while loading")]
        [SerializeField] private OnLoadingTickEvent _onLoadingTick = new OnLoadingTickEvent();
                
        [Tooltip("Event triggered when loading has completed")]
        [SerializeField] private OnLoadEndEvent _onLoadEnd = new OnLoadEndEvent();

        [Header("Unloading Events")]
        [Tooltip("Event triggered when unloading is started")]
        [SerializeField] private OnUnloadStartEvent _onUnloadStart = new OnUnloadStartEvent();
                
        [Tooltip("Event triggered each frame while unloading")]
        [SerializeField] private OnUnloadingTickEvent _onUnloadingTick = new OnUnloadingTickEvent();
                
        [Tooltip("Event triggered when unloading has completed")]
        [SerializeField] private OnUnloadEndEvent _onUnloadEnd = new OnUnloadEndEvent();
        
        [Tooltip("Event triggered after loading is completed and Active Scene has been set")]
        [SerializeField] private AfterLoadEvent _afterLoad = new AfterLoadEvent();
        
        [Tooltip("Event triggered after loading screen is hidden")]
        [SerializeField] private AfterLoadingScreenEvent _afterLoadingScreen = new AfterLoadingScreenEvent();
#endregion
        
#region " Internal Variables "
        private string _activeSceneName;
                
        private int _currentSceneIndex;

        private static readonly List<ISceneLoaderListener> _listeners = new List<ISceneLoaderListener>();

        private LoadData _loadData;
        
        private static AsyncOperationHandle<SceneInstance>? _loadingSceneHandle;

        private ILoadingTracker _loadingTracker;
                
        private int _sceneCount;

        private readonly List<AsyncOperationHandle<SceneInstance>> _sceneHandles = new List<AsyncOperationHandle<SceneInstance>>();

        private static readonly List<SceneLoader> _sceneLoaders = new List<SceneLoader>();
#endregion

#region " ISceneLoaderListener "
        public override void AfterLoad(AfterLoadData data)
        {
            Output($"[{nameof(AfterLoad)}] {_activeSceneName}");
        }

        public override void AfterLoadingScreen(AfterLoadingScreenData data)
        {
            Output($"[{nameof(AfterLoadingScreen)}] {_activeSceneName}");
        }
        
        public override void BeforeLoad(BeforeLoadData data)
        {
            Output($"[{nameof(BeforeLoad)}] {_activeSceneName}");
        }

        public override void BeforeLoadingScreen(BeforeLoadingScreenData data)
        {
            Output($"[{nameof(BeforeLoadingScreen)}] {_activeSceneName}");
        }
        
        public override void OnLoadEnd(OnLoadEndData data)
        {
            Output($"[{nameof(OnLoadEnd)}] {_activeSceneName}");
        }

        public override void OnLoadingTick(OnLoadingTickData data)
        {
            Output($"[{nameof(OnLoadingTick)}] {_activeSceneName}");
        }

        public override void OnLoadStart(OnLoadStartData data)
        {
            Output($"[{nameof(OnLoadStart)}] {_activeSceneName}");
        }

        public override void OnUnloadEnd(OnUnloadEndData data)
        {
            Output($"[{nameof(OnUnloadEnd)}] {_activeSceneName}");
        }

        public override void OnUnloadingTick(OnUnloadingTickData data)
        {
            Output($"[{nameof(OnUnloadingTick)}] {_activeSceneName}");
        }

        public override void OnUnloadStart(OnUnloadStartData data)
        {
            Output($"[{nameof(OnUnloadStart)}] {_activeSceneName}");
        }
#endregion

        public static void AddListeners(params ISceneLoaderListener[] listeners)
        {
            var lListeners = listeners.Where(x => false == _listeners.Contains(x));
            _listeners.AddRange(lListeners);
        }

        protected virtual void Awake()
        {
            Assert.IsTrue(_scenes.Count > 0, $"{nameof(_scenes)} has not been set");

            _sceneLoaders.Add(this);
        }

        protected virtual AfterLoadData CreateAfterLoadData()
        {
            return new AfterLoadData(this, _loadData.StartTime, _loadData.EndTime)
            {
                Time = _loadData.Time
            };
        }
        
        protected virtual AfterLoadingScreenData CreateAfterLoadingScreenData()
        {
            return new AfterLoadingScreenData(this, _loadData.StartTime, _loadData.EndTime)
            {
                Time = _loadData.Time
            };
        }
        
        protected virtual BeforeLoadData CreateBeforeLoadData()
        {
            return new BeforeLoadData(this, _loadData.StartTime, _loadData.EndTime)
            {
                Time = _loadData.Time
            };
        }

        protected virtual BeforeLoadingScreenData CreateBeforeLoadingScreenData()
        {
            return new BeforeLoadingScreenData(this, _loadData.StartTime, _loadData.EndTime)
            {
                Time = _loadData.Time
            };
        }
        
        protected virtual OnLoadEndData CreateOnLoadEndData()
        {
            return new OnLoadEndData(this, _loadData.StartTime, _loadData.EndTime)
            {
                Time = _loadData.Time
            };
        }
        
        protected virtual OnLoadingTickData CreateOnLoadingTickData()
        {
            return new OnLoadingTickData(this, _loadData.StartTime, _loadData.EndTime);
        }
        
        protected virtual OnLoadStartData CreateOnLoadStartData()
        {
            return new OnLoadStartData(this, _loadData.StartTime, _loadData.EndTime)
            {
                Time = _loadData.Time
            };
        }
        
        protected virtual OnUnloadEndData CreateOnUnloadEndData()
        {
            return new OnUnloadEndData(this, _loadData.StartTime, _loadData.EndTime)
            {
                Time = _loadData.Time
            };
        }
        
        protected virtual OnUnloadingTickData CreateOnUnloadingTickData()
        {
            return new OnUnloadingTickData(this, _loadData.StartTime, _loadData.EndTime);
        }
        
        protected virtual OnUnloadStartData CreateOnUnloadStartData()
        {
            return new OnUnloadStartData(this, _loadData.StartTime, _loadData.EndTime)
            {
                Time = _loadData.Time
            };
        }

        protected void DestroySelf()
        {
            Destroy(gameObject);
        }

        public void DontDestroy()
        {
            _dontDestroyOnLoad = true;
            
            // Only GameObjects without parents can be used with DontDestroyOnLoad
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Attempts to load scenes. 
        /// </summary>
        [ContextMenu("Load")]
        public void Load()
        {
            StartCoroutine(nameof(LoadAsync));
        }

        /// <summary>
        /// Attempts to load scenes. 
        /// </summary>
        public IEnumerator LoadAsync()
        {
            if (!CanExecuteLoad)
                yield break;

            IsLoading = true;

            _loadData = new LoadData(Time.time);

            _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            var lBeforeLoadData = CreateBeforeLoadData();
            beforeLoad(lBeforeLoadData);
            _beforeLoad?.Invoke(lBeforeLoadData);
            yield return StartCoroutine(NotifyListeners(
                x => x.BeforeLoad(lBeforeLoadData),
                x => x.BeforeLoadAsync(lBeforeLoadData)));

            yield return OpenLoadingScene();

            var lStartData = CreateOnLoadStartData();
            onLoadStart(lStartData);
            _onLoadStart?.Invoke(lStartData);
            yield return StartCoroutine(NotifyListeners(
                x => x.OnLoadStart(lStartData), 
                x => x.OnLoadStartAsync(lStartData)));

            yield return BeginLoading();

            var lEndData = CreateOnLoadEndData();
            onLoadEnd(lEndData);
            _onLoadEnd?.Invoke(lEndData);
            yield return StartCoroutine(NotifyListeners(
                x => x.OnLoadEnd(lEndData), 
                x => x.OnLoadEndAsync(lEndData)));
            
            if (_unloadPrevious)
            {
                if (_currentSceneIndex > -1)
                    SceneManager.UnloadSceneAsync(_currentSceneIndex);

                var lSceneLoaders = _sceneLoaders
                    .Where(sl => sl != this)
                    .Where(sl => sl.IsLoaded)
                    .ToArray();

                var lIndex = lSceneLoaders.Length;
                while (--lIndex >= 0)
                {
                    var lSceneLoader = lSceneLoaders[lIndex];
                    yield return lSceneLoader.UnloadAsync();
                }   
            }
            
            yield return CloseLoadingScene();

            while (false == IsLoaded)
                yield return null;

            var lAfterLoadData = CreateAfterLoadData();
            afterLoad(lAfterLoadData);
            _afterLoad?.Invoke(lAfterLoadData);
            yield return StartCoroutine(NotifyListeners(
                x => x.AfterLoad(lAfterLoadData), 
                x => x.AfterLoadAsync(lAfterLoadData)));
        }

        private IEnumerator CloseLoadingScene()
        {
            if (_minimumLoadingTime.Enabled)
                yield return new WaitUntil(() => Time.time - _loadData.StartTime >= _minimumLoadingTime.Value);
            
            if (_loadingSceneHandle.HasValue)
            {
                yield return _loadingTracker?.Close();
                _loadingTracker = null;

                _loadingSceneHandle = Addressables.UnloadSceneAsync(_loadingSceneHandle.Value);
                yield return _loadingSceneHandle;
                
                var lAfterLoadingScreenData = CreateAfterLoadingScreenData();
                afterLoadingScreen(lAfterLoadingScreenData);
                _afterLoadingScreen?.Invoke(lAfterLoadingScreenData);
                yield return StartCoroutine(NotifyListeners(
                    x => x.AfterLoadingScreen(lAfterLoadingScreenData), 
                    x => x.AfterLoadingScreenAsync(lAfterLoadingScreenData)));
            }

            _loadingSceneHandle = null;
        }

        private IEnumerator BeginLoading()
        {
            var lAdditiveHandles = _scenes.Select(addr =>
            {
                var lAdditiveHandle = Addressables.LoadSceneAsync(addr, LoadSceneMode.Additive);
                lAdditiveHandle.Completed += SceneLoadComplete;
                return lAdditiveHandle;
            });

            _sceneHandles.AddRange(lAdditiveHandles);

            var lTickData = CreateOnLoadingTickData();
            while (IsLoading)
            {
                onLoadingTick(lTickData);
                _onLoadingTick?.Invoke(lTickData);
                yield return StartCoroutine(NotifyListeners(
                    x => x.OnLoadingTick(lTickData), 
                    x => x.OnLoadingTickAsync(lTickData)));
                
                _loadData.Time = Time.time - lTickData.StartTime;
                lTickData.Time = _loadData.Time;
            }
        }

        private IEnumerator NotifyListeners(System.Action<ISceneLoaderListener> action, System.Func<ISceneLoaderListener, IEnumerator> actionAsync)
        {
            int lIndex = _listeners.Count;
            while (--lIndex >= 0)
            {
                var lListener = _listeners[lIndex];                
                action?.Invoke(lListener);
                yield return StartCoroutine(actionAsync?.Invoke(lListener));
            }
        }

        private IEnumerator OpenLoadingScene()
        {
            if (_loadingScene.Enabled && !_loadingSceneHandle.HasValue)
            {
                var lBeforeLoadingScreenData = CreateBeforeLoadingScreenData();
                beforeLoadingScreen(lBeforeLoadingScreenData);
                _beforeLoadingScreen?.Invoke(lBeforeLoadingScreenData);
                yield return StartCoroutine(NotifyListeners(
                    x => x.BeforeLoadingScreen(lBeforeLoadingScreenData), 
                    x => x.BeforeLoadingScreenAsync(lBeforeLoadingScreenData)));
                
                _loadingSceneHandle = Addressables.LoadSceneAsync(_loadingScene.Value, LoadSceneMode.Additive);
                yield return _loadingSceneHandle;

                _loadingTracker = FindObjectOfType<LoadingTracker>();
                
                var lDuration = _loadingFadeDuration.Enabled
                    ? _loadingFadeDuration.Value
                    : 0;
                
                yield return _loadingTracker?.Open(lDuration, this, _loadingSceneSortOrder);
            }
        }

        protected virtual void OnDestroy()
        {
            _sceneLoaders.Remove(this);
        }

        protected virtual void OnDisable()
        {
            RemoveListeners(this);
        }

        protected virtual void OnEnable()
        {
            AddListeners(this);
        }

        protected virtual void Output(string message)
        {
            if (!_loggerEnabled)
                return;
            
            Debug.Log(message, this);
        }
        
        public static void RemoveListeners(params ISceneLoaderListener[] listeners)
        {
            _listeners.RemoveAll(listeners.Contains);
        }
        
        private void SceneLoadComplete(AsyncOperationHandle<SceneInstance> handle)
        {
            _sceneCount += 1;

            IsLoading = _sceneCount < SceneCount;
            IsLoaded = _sceneCount == SceneCount;
            IsUnloaded = !IsLoaded;

            if (IsLoaded)
            {
                var lActiveScene = _sceneHandles[0].Result.Scene;
                _activeSceneName = lActiveScene.name;
                gameObject.name = $"{_activeSceneName} [Loaded Ref]";

                StartCoroutine(SetSceneAsActive(lActiveScene));
            }
        }
        
        private void SceneUnloadComplete(AsyncOperationHandle<SceneInstance> handle)
        {
            _sceneCount -= 1;

            IsUnloading = _sceneCount > 0 && _sceneCount < SceneCount;
            IsUnloaded = _sceneCount == 0;
            IsLoaded = !IsUnloaded;

            if (IsUnloaded)
            {
                gameObject.name = $"{_activeSceneName} [Unloaded Ref]";
            }
        }

        private IEnumerator SetSceneAsActive(Scene scene)
        {
            yield return new WaitUntil(() => _loadingSceneHandle.HasValue == false);
            
            if (_setFirstSceneAsActive)
                SceneManager.SetActiveScene(scene);
        }
        
        protected virtual IEnumerator Start()
        {
            if (_dontDestroyOnLoad)
                DontDestroy();

                if (_loadOnStart)
                yield return LoadAsync();
        }
        
        /// <summary>
        /// Attempts to unload scenes. 
        /// </summary>
        [ContextMenu("Unload")]
        public void Unload()
        {
            StartCoroutine(nameof(UnloadAsync));
        }

        /// <summary>
        /// Attempts to unload scenes. 
        /// </summary>
        public IEnumerator UnloadAsync()
        {
            if (!CanExecuteUnload)
                yield break;

            if (SceneManager.sceneCount <= SceneCount)
            {
                Debug.LogWarning($"Cannot unload scene(s). Doing so would reduce scene count below Unity required threshold.");
                yield break;
            }
            
            IsUnloading = true;
            
            _loadData = new LoadData(Time.time);

            var lStartData = CreateOnUnloadStartData();
            onUnloadStart(lStartData);
            _onUnloadStart?.Invoke(lStartData);
            yield return StartCoroutine(NotifyListeners(
                x => x.OnUnloadStart(lStartData), 
                x => x.OnUnloadStartAsync(lStartData)));
            
            yield return BeginUnload();

            _loadData.EndTime = Time.time;
            
            var lEndData = CreateOnUnloadEndData();
            onUnloadEnd(lEndData);
            _onUnloadEnd?.Invoke(lEndData);
            yield return StartCoroutine(NotifyListeners(
                x => x.OnUnloadEnd(lEndData), 
                x => x.OnUnloadEndAsync(lEndData)));
            
            if (IsUnloaded && _destroyOnUnload)
            {
                DestroySelf();
            }
        }

        private IEnumerator BeginUnload()
        {
            int lIndex = _sceneHandles.Count;
            while (--lIndex >= 0)
            {
                var lSceneHandle = _sceneHandles[lIndex];
                lSceneHandle = Addressables.UnloadSceneAsync(lSceneHandle, true);
                lSceneHandle.Completed += SceneUnloadComplete;
            }
            
            var lTickData = CreateOnUnloadingTickData();
            while (IsUnloading)
            {
                onUnloadingTick(lTickData);
                _onUnloadingTick?.Invoke(lTickData);
                yield return StartCoroutine(NotifyListeners(
                    x => x.OnUnloadingTick(lTickData), 
                    x => x.OnUnloadingTickAsync(lTickData)));
                
                _loadData.Time = Time.time - lTickData.StartTime;
                lTickData.Time = _loadData.Time;
            }
            
            _sceneHandles.Clear();
        }

        private struct LoadData
        {
            public float EndTime { get; set; }
            
            public float StartTime { get; }

            public float Time { get; set; }

            public LoadData(float startTime)
            {
                EndTime = -1f;
                StartTime = startTime;
                Time = 0f;
            }
        }
    }
}