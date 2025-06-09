using UnityEngine;
using Zenject;
using _Project.Scripts.Data.Configs;

namespace _Project.Scripts.Models.PlayerComponents
{
    public class AimCalculator
    {
        private readonly PlayerConfig _config;
        private readonly Camera _camera;

        [Inject]
        public AimCalculator(PlayerConfig config, Camera camera)
        {
            _config = config;
            _camera = camera;
        }

        public Vector2 DetermineAimDirection(Vector2 playerPosition, float lastDirX)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(playerPosition, _config.ShootRange, _config.EnemyLayerMask);
            
            if (hits.Length > 0)
            {
                Collider2D closest = null;
                float best = float.MaxValue;
                
                foreach (Collider2D hit in hits)
                {
                    float directionHit = ((Vector2)hit.transform.position - playerPosition).sqrMagnitude;
                    
                    if (directionHit < best)
                    {
                        best = directionHit;
                        closest = hit;
                    }
                }

                if (closest != null)
                    return (((Vector2)closest.transform.position) - playerPosition).normalized;
            }
            
            Vector3 screenPosition = Input.mousePosition;
            Vector3 worldPosition = _camera.ScreenToWorldPoint(screenPosition);
            Vector2 direction = (new Vector2(worldPosition.x, worldPosition.y) - playerPosition).normalized;
            
            if (Mathf.Approximately(direction.magnitude, 0f))
                return new Vector2(lastDirX, 0f);
            
            return direction;
        }
    }
}