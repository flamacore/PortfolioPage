namespace PortfolioViewer.Models
{
    /// <summary>
    /// Interface for all models in the PortfolioViewer application.
    /// </summary>
    public interface IModel
    {
        /// <summary>
        /// Initializes the model.
        /// </summary>
        void Init();
        
        /// <summary>
        ///  Updates the model.
        ///  </summary>
        void Update();
        
        /// <summary>
        ///  Disposes the model.
        ///  </summary>
        void Dispose();
    }
}