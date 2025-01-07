using System;
using JetBrains.Annotations;
using PortfolioViewer.Models.Impl;
using PortfolioViewer.Utilities;
using PortfolioViewer.Views.Impl;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PortfolioViewer.Controllers.Impl
{
    /// <summary>
    /// Controller for managing the liquids window in the PortfolioViewer application.
    /// Inherits from <see cref="ControllerBase"/>.
    /// </summary>
    public class LiquidsWindowController : ControllerBase
    {
        /// <summary>
        /// Array of light positions for each liquid material.
        /// </summary>
        private Vector2[] _lightPositions;

        /// <summary>
        /// Array of caustics levels for each liquid material.
        /// </summary>
        private float[] _causticsLevels;

        /// <summary>
        /// Model associated with the liquids window.
        /// </summary>
        private readonly LiquidsWindowModel _model = new ();
        
        /// <summary>
        /// View associated with the liquids window.
        /// </summary>
        private LiquidsWindowView _view;

        /// <summary>
        /// Initializes the controller, view and the associated model.
        /// </summary>
        public override void Init()
        {
            base.Init();
            _model.Init();
            _view = Object.FindAnyObjectByType<LiquidsWindowView>(FindObjectsInactive.Include);
            if (_view == null)
            {
                DebugLogger.LogError("LiquidsWindowView not found. Controller cannot be initialized.");
                return;
            }
            _view.Init();
            Initialize();
        }

        /// <summary>
        /// Initializes the liquid materials, light positions, and caustics levels.
        /// </summary>
        private void Initialize()
        {
            _view.CausticsChanged += SetCausticsLevel;
            _view.FillAmountChanged += SetFillAmount;
            _view.LightPositionChanged += SetLightPositionX;
            
            SetFillAmount(0.5f);
            _lightPositions = new Vector2[_view.LiquidMaterials.Length];
            _causticsLevels = new float[_view.LiquidMaterials.Length];
            for (var i = 0; i < _view.LiquidMaterials.Length; i++)
            {
                _lightPositions[i] = _view.LiquidMaterials[i].GetVector(_model.LightPositionShaderKey);
                _causticsLevels[i] = _view.LiquidMaterials[i].GetFloat(_model.CausticsShaderKey);
            }
            DebugLogger.Log($"Light Positions: {_lightPositions.Length}");
            SetLightPositionX(Vector2.zero);
        }

        /// <summary>
        /// Sets the fill amount for all liquid materials.
        /// </summary>
        /// <param name="fillAmount">The fill amount to set.</param>
        [UsedImplicitly]
        public void SetFillAmount(float fillAmount)
        {
            foreach (var liquidMaterial in _view.LiquidMaterials)
            {
                liquidMaterial.SetFloat(_model.FillAmountShaderKey, fillAmount);
            }
        }

        /// <summary>
        /// Sets the X position of the light for all liquid materials.
        /// Keeping their original x position and only adding the new x position.
        /// Y position remains the same.
        /// Just for demonstration purposes.
        /// </summary>
        [UsedImplicitly]
        public void SetLightPositionX(Vector2 x)
        {
            for (var index = 0; index < _view.LiquidMaterials.Length; index++)
            {
                var liquidMaterial = _view.LiquidMaterials[index];
                liquidMaterial.SetVector(_model.LightPositionShaderKey, new Vector2(x.x + _lightPositions[index].x, _lightPositions[index].y));
            }
        }

        /// <summary>
        /// Sets the caustics level for all liquid materials.
        /// Keeps the original caustics level and only adds the new caustics level.
        /// </summary>
        /// <param name="causticsLevel">The caustics level to set.</param>
        [UsedImplicitly]
        public void SetCausticsLevel(float causticsLevel)
        {
            for (var index = 0; index < _view.LiquidMaterials.Length; index++)
            {
                var liquidMaterial = _view.LiquidMaterials[index];
                liquidMaterial.SetFloat(_model.CausticsShaderKey, Mathf.Clamp(_causticsLevels[index] + causticsLevel, 0, Single.PositiveInfinity));
            }
        }
        
        /// <summary>
        ///  Disposes the controller.
        ///  Unsubscribes from all events.
        ///  Disposes the model.
        ///  Sets the view to null.
        ///  </summary>
        public override void Dispose()
        {
            base.Dispose();
            _view.CausticsChanged -= SetCausticsLevel;
            _view.FillAmountChanged -= SetFillAmount;
            _view.LightPositionChanged -= SetLightPositionX;
            _model.Dispose();
            _view = null;
        }
        
        /// <summary>
        ///  Disposes the controller on GC collect.
        ///  GC collect is usually called by Views explicitly.
        ///  </summary>
        ~LiquidsWindowController() => Dispose();
    }
}