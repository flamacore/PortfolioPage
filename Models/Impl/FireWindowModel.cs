using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PortfolioViewer.Utilities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
// ReSharper disable IdentifierTypo

namespace PortfolioViewer.Models.Impl
{
    /// <summary>
    /// Model class for managing the fire window in the PortfolioViewer application.
    /// Inherits from <see cref="ModelBase"/>.
    /// </summary>
    public class FireWindowModel : ModelBase
    {
        /// <summary>
        /// List of video frames as sprites.
        /// </summary>
        private readonly List<Sprite> _videoFrames = new();

        /// <summary>
        /// List of resource locations for the addressable assets.
        /// </summary>
        private readonly List<IResourceLocation> _locations = new();

        /// <summary>
        /// Index of the current frame.
        /// </summary>
        private int _currentFrameIndex;

        /// <summary>
        /// Key for the addressable resource.
        /// </summary>
        private string _addressableKey;

        /// <summary>
        /// Cancellation token source for frame load wait.
        /// </summary>
        private CancellationTokenSource _frameLoadWaitToken;

        /// <summary>
        /// Event triggered when all frames are loaded.
        /// </summary>
        public Action<string> OnAllFramesLoaded;

        /// <summary>
        /// Event triggered when the loading of addressable resources starts.
        /// </summary>
        public Action OnStartLoadingAddressables;

        /// <summary>
        /// Event triggered when the loading of addressable resources finishes.
        /// </summary>
        public Action<int> OnFinishedLoadingAddressables;

        /// <summary>
        /// Event triggered when the loading of video frames starts.
        /// </summary>
        public Action OnStartLoadingFrames;

        /// <summary>
        /// Event triggered to update the progress normalized.
        /// </summary>
        public Action<float> OnProgressNormalized;

        /// <summary>
        /// Gets the list of video frames.
        /// </summary>
        public List<Sprite> VideoFrames => _videoFrames;

        //Note here; I probably don't need to access every single one of these publicly but for the sake of conventions, I'll leave them as is and ignore the suggestion with pragma.
        // ReSharper disable MemberCanBePrivate.Global
        public const string SuccessStatusCode = "01";
        public const string SuccessStatusMessage = " All frames loaded successfully";
        public const string ErrorStatusCode = "02";
        public const string ErrorStatusMessage = " Error loading video frames, please restart the application";
        public const string CancelStatusCode = "03";
        public const string CancelStatusMessage = " Frame load wait cancelled";

        /// <summary>
        /// Disposes of the model resources.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            _frameLoadWaitToken?.Cancel();
        }

        /// <summary>
        /// Clears the loaded video frames and resource locations.
        /// </summary>
        public void Clear()
        {
            _frameLoadWaitToken?.Cancel();
            _locations.Clear();
            _videoFrames.Clear();
        }

        /// <summary>
        /// Loads the video frames from addressable assets.
        /// </summary>
        /// <param name="addressableKey">The key for the addressable resource.</param>
        public async Task LoadVideo(string addressableKey)
        {
            await Task.Yield(); // Just to unblock the main thread and make the method async
            _frameLoadWaitToken = new CancellationTokenSource();
            _addressableKey = addressableKey;
            _videoFrames.Clear();
            DebugLogger.Log("Loading video from addressables");

            OnStartLoadingAddressables?.Invoke();
            OnProgressNormalized?.Invoke(0.05f);
            AsyncOperationHandle<IList<IResourceLocation>> loadResourceLocationsAsync = Addressables.LoadResourceLocationsAsync(addressableKey, typeof(Sprite));
            loadResourceLocationsAsync.Completed += OnLocationsLoaded;
        }

        private async void OnLocationsLoaded(AsyncOperationHandle<IList<IResourceLocation>> handle)
        {
            _locations.AddRange(handle.Result);
            DebugLogger.Log($"Found {_locations.Count} resources under key {_addressableKey}");

            OnFinishedLoadingAddressables?.Invoke(_locations.Count);
            OnStartLoadingFrames?.Invoke();
            OnProgressNormalized?.Invoke(0.1f);

            for (var index = 0; index < _locations.Count; index++)
            {
                IResourceLocation location = _locations[index];
                AsyncOperationHandle<Sprite> asyncOperationHandle = Addressables.LoadAssetAsync<Sprite>(location);
                asyncOperationHandle.Completed += OnSpriteLoaded;

                if(index % 20 == 0)
                {
                    OnProgressNormalized?.Invoke((float)index / _locations.Count); // Update progress bar every 10 frames so it doesn't hog the thread.
                    await Task.Yield();
                }
            }

            string message = await WaitUntilAllFramesLoaded();
            OnAllFramesLoaded?.Invoke(message);
        }

        private void OnSpriteLoaded(AsyncOperationHandle<Sprite> operationHandle) => _videoFrames.Add(operationHandle.Result);

        /// <summary>
        /// Waits until all frames are loaded.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the status message.</returns>
        private async Task<string> WaitUntilAllFramesLoaded()
        {
            string message = SuccessStatusCode + SuccessStatusMessage;
            while (_videoFrames.Count < _locations.Count)
            {
                await Task.Delay(100);
            }
            if(_frameLoadWaitToken.Token.IsCancellationRequested)
            {
                DebugLogger.LogWarning(CancelStatusMessage);
                message = CancelStatusCode + CancelStatusMessage;
            }
            if(_videoFrames.Count != _locations.Count)
            {
                DebugLogger.LogError(ErrorStatusMessage);
                message = ErrorStatusCode + ErrorStatusMessage;
            }
            OnProgressNormalized?.Invoke(1);
            DebugLogger.Log(message);
            return message;
        }
    }
}