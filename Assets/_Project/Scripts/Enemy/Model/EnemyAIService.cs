using UnityEngine;
using _Project.Scripts.Data;
using _Project.Scripts.Enemy.Model.Features;
using _Project.Scripts.Player.Model;

namespace _Project.Scripts.Enemy.Model
{
    public class EnemyAIService
    {
        private readonly EnemyRotation _rotation;
        private readonly EnemyMovement _movement;
        private readonly EnemyAttack _attack;
        
        private readonly float _detectionRadius;

        public EnemyAIService(EnemyRotation rotation, EnemyMovement movement, EnemyAttack attack, float detectionRadius)
        {
            _rotation = rotation;
            _movement = movement;
            _attack = attack;
            _detectionRadius = detectionRadius;
        }
        
        public void Update(Transform enemyTransform, Rigidbody2D rigidbody2D, Animator animator, Transform playerTransform, PlayerModel playerModel)
        {
            if (playerTransform == null || playerModel == null)
                return;
            
            _rotation.FacePlayer(enemyTransform, playerTransform);
            
            if (_attack.IsAttacking)
            {
                rigidbody2D.velocity = Vector2.zero;
                
                animator.SetBool(CharacterAnimationParams.WalkingEnemy, false);
                
                return;
            }
            
            float distanceToPlayer = Vector2.Distance(enemyTransform.position, playerTransform.position);
            
            if (distanceToPlayer <= _attack.AttackDistance)
            {
                _attack.TryStartAttack(enemyTransform, playerTransform, rigidbody2D, animator);
                
                return;
            }
            
            if (distanceToPlayer <= _detectionRadius)
            {
                _movement.ChasePlayer(enemyTransform, playerTransform, animator);
                
                return;
            }
            
            _movement.ReturnToSpawn(enemyTransform, animator);
        }
        
        public void OnAttackEnded()
        {
            _attack.EndAttack();
        }
        
        public void OnDealDamage(Transform enemyTransform, Transform playerTransform, PlayerModel playerModel)
        {
            _attack.DealDamage(enemyTransform, playerTransform, playerModel);
        }
    }
}