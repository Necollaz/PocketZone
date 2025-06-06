using UnityEngine;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Pools;

namespace _Project.Scripts.Models.InventoryModel.Items
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Bullet : MonoBehaviour
    {
        private const string ObstacleLayer = "Obstacle";
        
        private Rigidbody2D _rigidbody;
        private Collider2D _collider;
        private Vector2 _direction;
        
        private int _damage;
        private float _speed;
        private float _lifetime;
        private float _spawnTime;

        public static BulletPool SharedPool;

        public void Initialize(float speed, int damage, float lifetime)
        {
            _speed = speed;
            _damage = damage;
            _lifetime = lifetime;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            
            _rigidbody.bodyType = RigidbodyType2D.Kinematic;
            _rigidbody.simulated = true;
            _rigidbody.gravityScale = 0f;
            _collider.isTrigger = true;
        }

        private void OnEnable()
        {
            _spawnTime = Time.time;
        }

        private void Update()
        {
            if (Time.time - _spawnTime >= _lifetime)
            {
                ReturnToPool();
                
                return;
            }

            _rigidbody.velocity = _direction * _speed;
        }

        public void SetDirection(Vector2 dir)
        {
            _direction = dir.normalized;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IDamageable enemy))
            {
                enemy.TakeDamage(_damage);
                ReturnToPool();
                
                return;
            }
            
            if (other.gameObject.layer == LayerMask.NameToLayer(ObstacleLayer))
            {
                ReturnToPool();
            }
        }

        private void ReturnToPool()
        {
            gameObject.SetActive(false);
            SharedPool?.ReturnToPool(this);
        }
    }
}