using UnityEngine;
using Zenject;
using _Project.Scripts.Views;

namespace _Project.Scripts.Spawners
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyView _monsterPrefab;
        [SerializeField] private Vector2 _spawnAreaMin;
        [SerializeField] private Vector2 _spawnAreaMax;
        [SerializeField] private int _monstersCount;
        
        [Inject] private DiContainer _container;

        private void Start()
        {
            Spawn();
        }

        public void Spawn()
        {
            for (int i = 0; i < _monstersCount; i++)
            {
                Vector2 randomPosition = GetRandomPointInZone();
                
                _container.InstantiatePrefab(_monsterPrefab.gameObject, randomPosition, Quaternion.identity, null);
            }
        }
        
        private Vector2 GetRandomPointInZone()
        {
            float x = Random.Range(_spawnAreaMin.x, _spawnAreaMax.x);
            float y = Random.Range(_spawnAreaMin.y, _spawnAreaMax.y);
            
            return new Vector2(x, y);
        }
    }
}