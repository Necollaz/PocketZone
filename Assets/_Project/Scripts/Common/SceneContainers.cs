using UnityEngine;

namespace _Project.Scripts.Common
{
    public static class SceneContainers
    {
        private static Transform _bulletContainer;
        private static Transform _enemyContainer;
        
        public static Transform BulletContainer
        {
            get
            {
                if (_bulletContainer == null)
                {
                    GameObject bulletContainer = new GameObject("BulletPoolContainer");
                    _bulletContainer = bulletContainer.transform;
                }
                
                return _bulletContainer;
            }
        }
        
        public static Transform EnemyContainer
        {
            get
            {
                if (_enemyContainer == null)
                {
                    GameObject enemiesContainer = new GameObject("EnemiesContainer");
                    _enemyContainer = enemiesContainer.transform;
                }
                
                return _enemyContainer;
            }
        }
    }
}