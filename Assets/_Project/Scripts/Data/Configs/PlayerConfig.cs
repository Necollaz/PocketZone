using UnityEngine;

namespace _Project.Scripts.Data.Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig", order = 0)]
    public class PlayerConfig : ScriptableObject
    {
        [Header("Movement")]
        [SerializeField] private float _speed = 5f;
        
        [Header("Health")]
        [SerializeField] private int _maxHealth = 100;
        
        [Header("Shooting")]
        [SerializeField] private int _bulletPoolSize = 20;
        [SerializeField] private float _shootCooldown = 0.5f;
        [SerializeField] private float _shootRange = 8f;
        [SerializeField] private LayerMask _enemyLayerMask;
        
        [Header("Damage")]
        [SerializeField] private int _bulletDamage = 20;
        [SerializeField] private float _bulletSpeed = 8f;
        [SerializeField] private float _bulletLifetime = 3f;
        
        public LayerMask EnemyLayerMask => _enemyLayerMask;
        public int MaxHealth => _maxHealth;
        public int BulletPoolSize => _bulletPoolSize;
        public int BulletDamage => _bulletDamage;
        public float Speed => _speed;
        public float ShootCooldown => _shootCooldown;
        public float ShootRange => _shootRange;
        public float BulletSpeed => _bulletSpeed;
        public float BulletLifetime => _bulletLifetime;
    }
}