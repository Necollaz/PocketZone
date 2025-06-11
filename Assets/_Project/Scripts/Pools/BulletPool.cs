using System.Collections.Generic;
using UnityEngine;
using _Project.Scripts.Common;
using _Project.Scripts.Weapon.Model;

namespace _Project.Scripts.Pools
{
    public class BulletPool
    {
        private readonly Bullet _bulletBulletPrefab;
        private readonly Transform _parentTransform;

        private readonly Queue<Bullet> _poolQueue = new();
        private readonly int _initialSize;
        private readonly int _bulletDamage;
        private readonly float _bulletSpeed;
        private readonly float _bulletLifetime;

        public BulletPool(Bullet bulletPrefab, int initialSize, float speed, int damage, float lifetime,
            Transform parent = null)
        {
            _bulletBulletPrefab = bulletPrefab;
            _initialSize = initialSize;
            _bulletSpeed = speed;
            _bulletDamage = damage;
            _bulletLifetime = lifetime;
            _parentTransform = parent ?? SceneContainers.BulletContainer;

            Bullet.SharedPool = this;

            Initialize();
        }

        private void Initialize()
        {
            for (int i = 0; i < _initialSize; i++)
            {
                Bullet bullet = Object.Instantiate(_bulletBulletPrefab, Vector3.zero, Quaternion.identity,
                    _parentTransform);

                bullet.gameObject.SetActive(false);
                bullet.Initialize(_bulletSpeed, _bulletDamage, _bulletLifetime);

                _poolQueue.Enqueue(bullet);
            }
        }

        public Bullet Get()
        {
            if (_poolQueue.Count > 0)
            {
                return _poolQueue.Dequeue();
            }
            else
            {
                Bullet bullet = Object.Instantiate(_bulletBulletPrefab, Vector3.zero, Quaternion.identity,
                    _parentTransform);

                bullet.transform.SetParent(_parentTransform);
                bullet.Initialize(_bulletSpeed, _bulletDamage, _bulletLifetime);

                return bullet;
            }
        }

        public void ReturnToPool(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
            _poolQueue.Enqueue(bullet);
        }
    }
}