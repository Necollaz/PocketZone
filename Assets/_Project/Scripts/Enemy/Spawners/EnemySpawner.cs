using UnityEngine;
using Zenject;
using _Project.Scripts.Common;
using _Project.Scripts.Enemy.View;

namespace _Project.Scripts.Enemy.Spawners
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyView _monsterPrefab;
        [SerializeField] private Vector2 _spawnAreaMin;
        [SerializeField] private Vector2 _spawnAreaMax;
        [SerializeField] private LayerMask _obstacleLayer;
        [SerializeField] private int _monstersCount;
        [SerializeField] private float _spawnClearance = 0.5f;
        
        [Inject] private DiContainer _container;
        
        private void Start()
        {
            Spawn();
        }

        private void Spawn()
        {
            for (int i = 0; i < _monstersCount; i++)
            {
                Vector2 position = Vector2.zero;
                int attempts = 0;
                int maxCountAttempts = 100;
                bool valid = false;

                while (attempts < maxCountAttempts && !valid)
                {
                    position = GetRandomPointInZone();
                    Collider2D hit = Physics2D.OverlapCircle(position, _spawnClearance, _obstacleLayer);
                    
                    if (hit == null)
                        valid = true;
                    else
                        attempts++;
                }

                if (!valid)
                {
                    continue;
                }

                _container.InstantiatePrefab(_monsterPrefab.gameObject, position, Quaternion.identity, SceneContainers.EnemyContainer);
            }
        }
        
        private Vector2 GetRandomPointInZone()
        {
            float x = Random.Range(_spawnAreaMin.x, _spawnAreaMax.x);
            float y = Random.Range(_spawnAreaMin.y, _spawnAreaMax.y);
            
            return new Vector2(x, y);
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Vector3 center = ((Vector3)_spawnAreaMin + (Vector3)_spawnAreaMax) * 0.5f;
            Vector3 size = new Vector3(_spawnAreaMax.x - _spawnAreaMin.x, _spawnAreaMax.y - _spawnAreaMin.y, 0f);
            Gizmos.DrawWireCube(center, size);
        }
    }
}