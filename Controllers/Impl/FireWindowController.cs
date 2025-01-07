using PortfolioViewer.Models.Impl;
using PortfolioViewer.Utilities;
using PortfolioViewer.Views.Impl;
using UnityEngine;

namespace PortfolioViewer.Controllers.Impl
{
    /// <summary>
    /// Controller class for managing the fire window in the PortfolioViewer application.
    /// Inherits from <see cref="ControllerBase"/>.
    /// </summary>
    public class FireWindowController : ControllerBase
    {
        /// <summary>
        /// Key for the video addressable resource.
        /// </summary>
        private const string VideoAddressableKey = "FireVideo";

        /// <summary>
        /// Model for the fire window.
        /// </summary>
        private readonly FireWindowModel _model = new ();

        /// <summary>
        /// View for the fire window.
        /// </summary>
        private FireWindowView _view;

        /// <summary>
        /// Counter for the number of retry attempts.
        /// </summary>
        private int _tryCount;

        /// <summary>
        /// Initializes the controller.
        /// </summary>
        public override void Init()
        {
            base.Init();
            _model.Init();
            _view = Object.FindAnyObjectByType<FireWindowView>();
            if (_view == null)
            {
                DebugLogger.LogError("FireWindowView not found. Controller cannot be initialized.");
                return;
            }
            _view.Init();
            Initialize();
        }

        /// <summary>
        /// Initializes the model and sets up event handlers.
        /// </summary>
        private void Initialize()
        {
            _view.OnLoadButtonClicked += OnPlayButtonClicked;
        }
        
        private async void OnPlayButtonClicked()
        {
            _model.Clear();
            _model.OnStartLoadingAddressables += OnStartLoadingAddressables;
            _model.OnFinishedLoadingAddressables += OnFinishedLoadingAddressables;
            _model.OnStartLoadingFrames += OnStartLoadingFrames;
            _model.OnAllFramesLoaded += OnAllFramesLoaded;
            _model.OnProgressNormalized += OnProgressNormalized;
            await _model.LoadVideo(VideoAddressableKey);
        }
        
        private void OnProgressNormalized(float progress)
        {
            _view.LoadingIndicator.currentPercent = progress;
            _view.LoadingIndicator.UpdateUI();
        }

        /// <summary>
        /// Event handler for when the loading of video frames starts.
        /// </summary>
        private void OnStartLoadingFrames() => _view.UpdateStatus("Loading video frames");

        /// <summary>
        /// Event handler for when the loading of addressable resources finishes.
        /// </summary>
        /// <param name="count">The number of resources found.</param>
        private void OnFinishedLoadingAddressables(int count) => _view.UpdateStatus($"Found {count} resources under key {VideoAddressableKey}");

        /// <summary>
        /// Event handler for when the loading of addressable resources starts.
        /// </summary>
        private void OnStartLoadingAddressables() => _view.UpdateStatus("Loading video from addressables");

        /// <summary>
        /// Event handler for when all video frames are loaded.
        /// </summary>
        /// <param name="s">The status string.</param>
        private void OnAllFramesLoaded(string s)
        {
            if(s.Contains(FireWindowModel.SuccessStatusCode))
            {
                _view.SetVideo(_model.VideoFrames);
                _view.PlayVideo();
            }
            else
            {
                // Try again
                if (_tryCount < 3)
                {
                    _tryCount++;
                    Initialize();
                }
                else
                {
                    _view.ShowError();
                }
            }
        }

        /// <summary>
        /// Disposes of the controller resources.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            _view.OnLoadButtonClicked -= OnPlayButtonClicked;
            _model.OnStartLoadingAddressables -= OnStartLoadingAddressables;
            _model.OnFinishedLoadingAddressables -= OnFinishedLoadingAddressables;
            _model.OnStartLoadingFrames -= OnStartLoadingFrames;
            _model.OnAllFramesLoaded -= OnAllFramesLoaded;
            _model.Dispose();
            _view.Dispose();
        }

        /// <summary>
        /// Finalizer for the <see cref="FireWindowController"/> class.
        /// </summary>
        ~FireWindowController() => Dispose();
    }
}