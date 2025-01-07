using System;
using PortfolioViewer.Utilities;
using UnityEngine;

namespace PortfolioViewer.Views
{
    /// <summary>
    /// Base class for all views in the PortfolioViewer application.
    /// Inherits from <see cref="MonoBehaviour"/> and implements <see cref="IView"/>.
    /// </summary>
    public class ViewBase : MonoBehaviour, IView
    {
        /// <summary>
        /// Initializes the view.
        /// </summary>
        public virtual void Init()
        {
            DebugLogger.Log($" {GetType().Name} Initialized");
        }

        /// <summary>
        /// Disposes of the view resources.
        /// </summary>
        public virtual void Dispose()
        {
            DebugLogger.Log($" {GetType().Name} Disposed");
        }

        /// <summary>
        /// Called when the object is destroyed.
        /// </summary>
        private void OnDestroy()
        {
            Dispose();
            GC.Collect();
        }

        /// <summary>
        /// Called when the application quits.
        /// </summary>
        private void OnApplicationQuit()
        {
            Dispose();
            GC.Collect();
        }

        /// <summary>
        /// Called when the object is disabled.
        /// </summary>
        private void OnDisable()
        {
            Dispose();
            GC.Collect();
        }
    }
}