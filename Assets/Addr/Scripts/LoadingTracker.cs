using System.Collections;
using Studio.OverOne.Addr.Abstractions;
using Studio.OverOne.Addr.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Studio.OverOne.Addr
{
    internal sealed class LoadingTracker : MonoBehaviour, ILoadingTracker
    {
        [Header("References")]
        [Tooltip("Reference to the CanvasGroup used to handle fading")]
        [SerializeField] private CanvasGroup _canvasGroup;

        [Tooltip("Reference to the slider used to track loading progress")]
        [SerializeField] private Slider _progressSlider;

        private float _fadeDuration;
        
        private ISceneLoader _sceneLoader;
        
        /// <summary>
        /// Fades the CanvasGroup out over time
        /// </summary>
        public IEnumerator Close()
        {
            _sceneLoader = null;
            
            _progressSlider?.gameObject.SetActive(false);

            yield return _canvasGroup.Fade(0, _fadeDuration);
        }
        
        /// <summary>
        /// Fades the CanvasGroup in over time
        /// </summary>
        public IEnumerator Open(float fadeDuration, ISceneLoader loader, Optional<int> sortOrder)
        {
            _fadeDuration = fadeDuration;
            _sceneLoader = loader;

            if (sortOrder.Enabled && _canvasGroup.TryGetComponent<Canvas>(out var lCanvas))
                lCanvas.sortingOrder = sortOrder.Value;

            yield return _canvasGroup.Fade(1, fadeDuration);
            
            _progressSlider?.gameObject.SetActive(true);
        }

        private void Start()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = true;
            
            _progressSlider?.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_progressSlider == null || _sceneLoader == null)
                return;

            _progressSlider.value = _sceneLoader.Progress;
        }
    }
}