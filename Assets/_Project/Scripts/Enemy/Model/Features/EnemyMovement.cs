using UnityEngine;
using _Project.Scripts.Data;

namespace _Project.Scripts.Enemy.Model.Features
{
    public class EnemyMovement
    {
        private readonly float _moveSpeed;
        private readonly Vector2 _spawnPosition;

        public EnemyMovement(float moveSpeed, Vector2 spawnPosition)
        {
            _moveSpeed = moveSpeed;
            _spawnPosition = spawnPosition;
        }
        
        public void ChasePlayer(Transform enemyTransform, Transform playerTransform, Animator animator)
        {
            animator.SetBool(CharacterAnimationParams.WalkingEnemy, true);

            Vector2 currentPosition = enemyTransform.position;
            Vector2 targetPosition = playerTransform.position;
            Vector2 nextPosition = Vector2.MoveTowards(currentPosition, targetPosition, _moveSpeed * Time.deltaTime);
            enemyTransform.position = nextPosition;
        }
        
        public void ReturnToSpawn(Transform enemyTransform, Animator animator)
        {
            Vector2 currentPosition = enemyTransform.position;
            float distanceToSpawn = Vector2.Distance(currentPosition, _spawnPosition);

            if (distanceToSpawn < 0.01f)
            {
                animator.SetBool(CharacterAnimationParams.WalkingEnemy, false);
                
                return;
            }

            animator.SetBool(CharacterAnimationParams.WalkingEnemy, true);
            
            Vector2 nextPosition = Vector2.MoveTowards(currentPosition, _spawnPosition, _moveSpeed * Time.deltaTime);
            enemyTransform.position = nextPosition;
        }
    }
}