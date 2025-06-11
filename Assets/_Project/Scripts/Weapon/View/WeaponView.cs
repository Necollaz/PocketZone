using UnityEngine;
using _Project.Scripts.Pools;
using _Project.Scripts.Weapon.Model;

namespace _Project.Scripts.Weapon.View
{
    public class WeaponView : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private int _poolSize = 20;

        private BulletPool _bulletPool;
        private float _bulletSpeed;
        private float _bulletLifetime;
        private int _bulletDamage;
        
        public void InitializePool(int poolSize, float bulletSpeed, int bulletDamage, float bulletLifetime)
        {
            _poolSize = poolSize;
            _bulletSpeed = bulletSpeed;
            _bulletDamage = bulletDamage;
            _bulletLifetime = bulletLifetime;

            _bulletPool = new BulletPool(_bulletPrefab, _poolSize, _bulletSpeed, _bulletDamage, _bulletLifetime, null);
            Bullet.SharedPool = _bulletPool;
        }
        
        public void Shoot(Vector2 direction)
        {
            Bullet bullet = _bulletPool.Get();
            bullet.transform.position = _spawnPoint.position;
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f);

            bullet.Initialize(_bulletSpeed, _bulletDamage, _bulletLifetime);
            bullet.SetDirection(direction);
            bullet.gameObject.SetActive(true);
        }
    }
}