using PortfolioViewer.Models.Impl;
using PortfolioViewer.Views.Impl;
using UnityEngine;

namespace PortfolioViewer.Controllers.Impl
{
    public class EmbergenWindowController : ControllerBase
    {
        private Vector3 _initialLightPosition;
        private Vector2 _initialMousePosition;
        private readonly EmbergenWindowModel _model = new ();
        private EmbergenWindowView _view;
        private EmbergenWindowDragView _dragView;

        public override void Init()
        {
            base.Init();
            _model.Init();
            _view = Object.FindAnyObjectByType<EmbergenWindowView>();
            if (_view == null)
            {
                Debug.LogError("EmbergenWindowView not found. Controller cannot be initialized.");
                return;
            }
            _view.Init();
            _dragView = Object.FindAnyObjectByType<EmbergenWindowDragView>();
            if (_dragView == null)
            {
                Debug.LogError("EmbergenWindowDragView not found. Controller cannot be initialized.");
                return;
            }
            _dragView.Init();
            Initialize();
        }

        private void Initialize()
        {
            _view.FlameColorChanged += SetFlameColor;
            _view.EmissiveIntensityChanged += SetEmissiveIntensity;
            _view.LightIntensityChanged += SetLightIntensity;
            _view.LightColorChanged += SetLightColor;

            _dragView.DragStarted += OnDragStarted;
            _dragView.DragEnded += OnDragEnded;
            _dragView.Dragged += OnDragged;
        }

        private void SetFlameColor(Color color)
        {
            foreach (Material material in _view.EmbergenMaterials)
            {
                material.SetColor(_model.FlameColorShaderKey, color);
            }
        }

        private void SetEmissiveIntensity(float intensity)
        {
            foreach (Material material in _view.EmbergenMaterials)
            {
                material.SetFloat(_model.EmissiveIntensityShaderKey, intensity);
            }
        }

        private void SetLightIntensity(float intensity)
        {
            foreach (GameObject lightObject in _view.LightObjects)
            {
                lightObject.GetComponent<Light>().intensity = intensity;
            }
        }

        private void SetLightColor(Color color)
        {
            foreach (GameObject lightObject in _view.LightObjects)
            {
                lightObject.GetComponent<Light>().color = color;
            }
        }

        private void OnDragStarted(Vector2 position)
        {
            _initialMousePosition = position;
            _initialLightPosition = _view.LightPivot.transform.localPosition;
        }

        private void OnDragEnded(Vector2 position)
        {
            // Optionally, you can add any logic needed when the drag ends
        }

        private void OnDragged(Vector2 position)
        {
            Vector2 mouseDelta = position - _initialMousePosition;
            mouseDelta *= _dragView.DragSpeed;
            Vector3 newPosition = _initialLightPosition + new Vector3(mouseDelta.x, mouseDelta.y, 0);
            newPosition.x = Mathf.Clamp(newPosition.x, _dragView.MinDragPosition.x, _dragView.MaxDragPosition.x);
            newPosition.y = Mathf.Clamp(newPosition.y, _dragView.MinDragPosition.y, _dragView.MaxDragPosition.y);
            _view.LightPivot.transform.localPosition = newPosition;
        }
    }
}