using UnityEngine;
using _Project.Scripts.Data;
using _Project.Scripts.Models.PlayerComponents;

namespace _Project.Scripts.Models.EnemyModel.Features
{
    public class EnemyAttack
    {
        private readonly int _attackDamage;
        private readonly float _attackCooldown;
        private readonly float _attackDistance;

        private float _lastAttackTime = -Mathf.Infinity;
        private bool _isAttacking = false;

        public EnemyAttack(int attackDamage, float attackCooldown, float attackDistance)
        {
            _attackDamage = attackDamage;
            _attackCooldown = attackCooldown;
            _attackDistance = attackDistance;
        }

        public float AttackDistance => _attackDistance;
        public bool IsAttacking => _isAttacking;

        public void TryStartAttack(Transform enemyTransform, Transform playerTransform, Rigidbody2D rigidbody2D, Animator animator)
        {
            float timeSinceLast = Time.time - _lastAttackTime;
            
            if (timeSinceLast < _attackCooldown)
            {
                animator.SetBool(CharacterAnimationParams.WalkingEnemy, false);
                
                return;
            }

            float currentDistance = Vector2.Distance(enemyTransform.position, playerTransform.position);
            
            if (currentDistance > _attackDistance)
            {
                return;
            }
            
            _isAttacking = true;
            rigidbody2D.velocity = Vector2.zero;
            
            animator.SetBool(CharacterAnimationParams.WalkingEnemy, false);
            animator.SetTrigger(CharacterAnimationParams.EnemyAttack);
        }
        
        public void EndAttack()
        {
            if (!_isAttacking)
                return;

            _isAttacking = false;
            _lastAttackTime = Time.time;
        }
        
        public void DealDamage(Transform enemyTransform, Transform playerTransform, Player player)
        {
            if (player == null)
                return;

            float currentDistance = Vector2.Distance(enemyTransform.position, playerTransform.position);
            
            if (currentDistance <= _attackDistance + 0.1f)
            {
                player.TakeDamage(_attackDamage);
            }
        }
    }
}