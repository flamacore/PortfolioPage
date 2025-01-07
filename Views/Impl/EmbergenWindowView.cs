using JetBrains.Annotations;
using UnityEngine;

namespace PortfolioViewer.Views.Impl
{
    public class EmbergenWindowView : ViewBase
    {
        [SerializeField] private Material[] _embergenMaterials;
        [SerializeField] private MeshRenderer[] _embergenMeshRenderers;
        [SerializeField] private GameObject[] _lightObjects;
        [SerializeField] private GameObject _lightPivot;
        [SerializeField] private Gradient _lightColorGradient;
        [SerializeField] private Gradient _flameColorGradient;
        
        private Material[] _materialCopies;
        public Material[] EmbergenMaterials => _materialCopies;
        public GameObject[] LightObjects => _lightObjects;
        public GameObject LightPivot => _lightPivot;
        
        public event System.Action<Color> FlameColorChanged;
        public event System.Action<float> EmissiveIntensityChanged;
        public event System.Action<float> LightIntensityChanged;
        public event System.Action<Color> LightColorChanged;

        public override void Init()
        {
            base.Init();
            _materialCopies = new Material[_embergenMaterials.Length];
            for (int i = 0; i < _embergenMaterials.Length; i++)
            {
                _materialCopies[i] = new Material(_embergenMaterials[i]);
            }
            for (int i = 0; i < _embergenMeshRenderers.Length; i++)
            {
                _embergenMeshRenderers[i].material = _materialCopies[i];
            }
        }

        [UsedImplicitly]
        public void SetFlameColor(float gradientValue)
        {
            Color color = _flameColorGradient.Evaluate(gradientValue);
            FlameColorChanged?.Invoke(color);
        }

        [UsedImplicitly]
        public void SetEmissiveIntensity(float intensity)
        {
            EmissiveIntensityChanged?.Invoke(intensity);
        }
        
        [UsedImplicitly]
        public void SetLightIntensity(float intensity)
        {
            LightIntensityChanged?.Invoke(intensity);
        }
        
        [UsedImplicitly]
        public void SetLightColor(float gradientValue)
        {
            Color color = _lightColorGradient.Evaluate(gradientValue);
            LightColorChanged?.Invoke(color);
        }

        public override void Dispose()
        {
            base.Dispose();
            foreach (Material material in _materialCopies)
            {
                Destroy(material);
            }
        }
    }
}