using PortfolioViewer.Utilities;

namespace PortfolioViewer.Controllers
{
    /// <summary>
    /// Base class for all controllers in the PortfolioViewer application.
    /// </summary>
    public class ControllerBase : IController
    {
        /// <summary>
        /// Initializes the controller.
        /// This method can be overridden by derived classes to provide custom initialization logic.
        /// It's advisable to call the base method in the overridden method to ensure that the controller is initialized correctly
        /// and throws the debug.
        ///
        /// All controllers initialize the model and view associated with them.
        /// </summary>
        public virtual void Init()
        {
            DebugLogger.Log($"{GetType().Name} Initialized");
        }
        
        /// <summary>
        ///  Disposes the controller.
        ///  This method can be overridden by derived classes to provide custom disposal logic.
        ///  It's advisable to call the base method in the overridden method to ensure that the controller is disposed correctly
        ///  and throws the debug.
        ///  </summary>
        public virtual void Dispose()
        {
            DebugLogger.Log($"{GetType().Name} Disposed");
        }
    }
}