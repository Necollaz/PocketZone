using UnityEngine;
using Zenject;
using _Project.Scripts.Data.Configs;

namespace _Project.Scripts.Player.Model.Features
{
    public class AimCalculator
    {
        private readonly PlayerConfig _config;

        [Inject]
        public AimCalculator(PlayerConfig config)
        {
            _config = config;
        }

        public Vector2? GetTargetDirection(Vector2 playerPosition)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(playerPosition, _config.ShootRange, _config.EnemyLayerMask);
            
            if (hits.Length == 0)
                return null;

            Collider2D closest = null;
            float bestDistance = float.MaxValue;
            
            foreach (Collider2D hit in hits)
            {
                float distance = ((Vector2)hit.transform.position - playerPosition).sqrMagnitude;
                
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    closest = hit;
                }
            }

            return (((Vector2)closest.transform.position) - playerPosition).normalized;
        }
    }
}