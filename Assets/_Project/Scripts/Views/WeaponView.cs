using UnityEngine;
using _Project.Scripts.Models.InventoryModel.Items;
using _Project.Scripts.Pools;

namespace _Project.Scripts.Views
{
    public class WeaponView : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private int _poolSize = 20;

        private BulletPool _bulletPool;
        
        public void InitializePool(int poolSize, float bulletSpeed, int bulletDamage, float bulletLifetime)
        {
            _poolSize = poolSize;
            _bulletPool = new BulletPool(_bulletPrefab, _poolSize, bulletSpeed, bulletDamage, bulletLifetime, null);
        }
        
        public void Shoot(Vector2 dir)
        {
            Bullet bullet = _bulletPool.Get();
            bullet.transform.position = _spawnPoint.position;
            bullet.transform.rotation = Quaternion.identity;

            bullet.SetDirection(dir);
            bullet.gameObject.SetActive(true);
        }
    }
}