using UnityEngine;

namespace PortfolioViewer.Models.Impl
{
    public class EmbergenWindowModel : ModelBase
    {
        private static readonly int FlameColor = Shader.PropertyToID("_Color");
        private static readonly int EmissiveIntensity = Shader.PropertyToID("_EmissiveMultiplier");

        public int FlameColorShaderKey => FlameColor;
        public int EmissiveIntensityShaderKey => EmissiveIntensity;
    }
}