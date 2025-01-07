using UnityEngine;

namespace PortfolioViewer.Models.Impl
{
    /// <summary>
    /// Model for managing the liquids window in the PortfolioViewer application.
    /// Inherits from <see cref="ModelBase"/>.
    /// </summary>
    public class LiquidsWindowModel : ModelBase
    {
        //Shader property IDs
        private static readonly int FillAmount = Shader.PropertyToID("Fill_Amount");
        private static readonly int LightPosition = Shader.PropertyToID("Light_Position");
        private static readonly int Caustics = Shader.PropertyToID("Inner_Glow");
        
        // Getters for shader property IDs
        public int FillAmountShaderKey => FillAmount;
        public int LightPositionShaderKey => LightPosition;
        public int CausticsShaderKey => Caustics;
    }
}