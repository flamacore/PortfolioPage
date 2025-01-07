namespace PortfolioViewer.Controllers
{
    /// <summary>
    /// Interface for all controllers in the PortfolioViewer application.
    /// </summary>
    public interface IController
    {
        /// <summary>
        /// Initializes the controller.
        /// </summary>
        void Init();
        
        /// <summary>
        ///  Disposes the controller.
        ///  </summary>
        void Dispose();
    }
}