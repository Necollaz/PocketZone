using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Scripts.Player.View
{
    [RequireComponent(typeof(Canvas))]
    public class HealthView : MonoBehaviour
    {
        [SerializeField] private Slider _healthSlider;
        
        private Canvas _canvas;
        
        [Inject] private Camera _worldCamera;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            
            _canvas.renderMode = RenderMode.WorldSpace;
            _canvas.worldCamera = _worldCamera;
        }
        
        public void Initialize(int currentHealth, int maxHealth)
        {
            _healthSlider.maxValue = maxHealth;
            _healthSlider.value = currentHealth;
        }
        
        public void UpdateHealth(int currentHealth)
        {
            _healthSlider.value = currentHealth;
        }
    }
}