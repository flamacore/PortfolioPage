using System;
using JetBrains.Annotations;
using PortfolioViewer.Utilities;
using UnityEngine;

namespace PortfolioViewer.Views.Impl
{
    /// <summary>
    /// View class for managing the liquids window in the PortfolioViewer application.
    /// Inherits from <see cref="ViewBase"/>.
    /// </summary>
    public class LiquidsWindowView : ViewBase
    {
        /// <summary>
        /// Array of materials representing the liquids.
        /// </summary>
        [SerializeField] private Material[] liquidMaterials;

        /// <summary>
        /// Property for accessing the liquid materials.
        /// </summary>
        public Material[] LiquidMaterials => liquidMaterials;

        // Events for the controller to subscribe to

        /// <summary>
        /// Event triggered when the fill amount changes.
        /// </summary>
        public event Action<float> FillAmountChanged;

        /// <summary>
        /// Event triggered when the light position changes.
        /// </summary>
        public event Action<Vector2> LightPositionChanged;

        /// <summary>
        /// Event triggered when the caustics (inner glow) changes.
        /// </summary>
        public event Action<float> CausticsChanged;

        /// <summary>
        /// Sets the fill amount and triggers the <see cref="FillAmountChanged"/> event.
        /// </summary>
        /// <param name="fillAmount">The new fill amount.</param>
        [UsedImplicitly]
        public void SetFillAmount(float fillAmount)
        {
            FillAmountChanged?.Invoke(fillAmount);
        }

        /// <summary>
        /// Sets the light position and triggers the <see cref="LightPositionChanged"/> event.
        /// </summary>
        /// <param name="lightPosition">The new light position.</param>
        [UsedImplicitly]
        public void SetLightPosition(float lightPosition)
        {
            LightPositionChanged?.Invoke(new Vector2(lightPosition, 0));
        }

        /// <summary>
        /// Sets the caustics (inner glow) and triggers the <see cref="CausticsChanged"/> event.
        /// </summary>
        /// <param name="caustics">The new caustics value.</param>
        [UsedImplicitly]
        public void SetCaustics(float caustics)
        {
            CausticsChanged?.Invoke(caustics);
        }
    }
}