using UnityEngine;
using Zenject;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Models.EnemyModel;
using _Project.Scripts.Models.InventoryModel.Items;

namespace _Project.Scripts.Views
{
    [RequireComponent(typeof(Collider2D))]
    public class EnemyView : MonoBehaviour, IDamageable
    {
        [SerializeField] private ItemPickup _dropPrefab;
        [SerializeField] private HealthView _healthView;
        [SerializeField] private int _maxHealth = 50;

        private Enemy _enemy;
        private Collider2D _collider;
        private DiContainer _container;
        
        [Inject]
        public void Construct(DiContainer container)
        {
            _container = container;
        }
        
        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            
            _collider.isTrigger = false;
            
            _enemy = new Enemy(_maxHealth);

            _enemy.HealthChanged += OnHealthChanged;
            _enemy.Died += OnDied;

            _healthView.Initialize(_enemy.CurrentHealth, _enemy.MaxHealth);
        }

        private void OnDestroy()
        {
            _enemy.HealthChanged -= OnHealthChanged;
            _enemy.Died -= OnDied;
        }

        private void OnHealthChanged(int newHealth)
        {
            _healthView.UpdateHealth(newHealth);
        }

        private void OnDied()
        {
            _container.InstantiatePrefab(_dropPrefab.gameObject, transform.position, Quaternion.identity, null);
            Destroy(gameObject);
        }

        public void TakeDamage(int amount)
        {
            _enemy.TakeDamage(amount);
        }
    }
}