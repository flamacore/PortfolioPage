using PortfolioViewer.Utilities;

namespace PortfolioViewer.Models
{
    /// <summary>
    /// Base class for all models in the PortfolioViewer application.
    /// Implements the IModel interface.
    /// </summary>
    public class ModelBase : IModel
    {
        /// <summary>
        /// Initializes the model.
        /// This method can be overridden by derived classes to provide custom initialization logic.
        /// </summary>
        public virtual void Init()
        {
            DebugLogger.Log($"{GetType().Name} Initialized");
        }
        
        /// <summary>
        ///  Updates the model.
        ///  This method can be overridden by derived classes to provide custom update logic.
        ///  </summary>
        public virtual void Update()
        {
            DebugLogger.Log($"{GetType().Name} Updated");
        }
        
        /// <summary>
        ///  Disposes the model.
        ///  This method can be overridden by derived classes to provide custom disposal logic.
        ///  </summary>
        public virtual void Dispose()
        {
            DebugLogger.Log($"{GetType().Name} Disposed");
        }
    }
}