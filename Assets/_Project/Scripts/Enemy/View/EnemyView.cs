using UnityEngine;
using Zenject;
using _Project.Scripts.Common.Interfaces;
using _Project.Scripts.Enemy.Model;
using _Project.Scripts.Inventory.Model.Items;
using _Project.Scripts.Player.View;

namespace _Project.Scripts.Enemy.View
{
    [RequireComponent(typeof(Collider2D))]
    public class EnemyView : MonoBehaviour, IDamageable
    {
        [SerializeField] private ItemPickup _dropPrefab;
        [SerializeField] private HealthView _healthView;
        [SerializeField] private int _maxHealth = 50;

        private LivingEntity _entity;
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
            
            _entity = new LivingEntity(_maxHealth);

            _entity.HealthChanged += OnHealthChanged;
            _entity.Died += OnDied;

            _healthView.Initialize(_entity.CurrentHealth, _entity.MaxHealth);
        }

        private void OnDestroy()
        {
            _entity.HealthChanged -= OnHealthChanged;
            _entity.Died -= OnDied;
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
            _entity.TakeDamage(amount);
        }
    }
}