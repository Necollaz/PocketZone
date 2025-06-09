using UnityEngine;

namespace _Project.Scripts.Data
{
    public static class CharacterAnimationParams
    {
        private const string IdleDirectionParam = "IdleDirection";
        private const string WalkLeftPlayerParam = "isWalkLeft";
        private const string WalkRightPlayerParam = "isWalkRight";
        private const string WalkEnemyParam = "isWalk";
        private const string EnemyAttackParam = "Attack";
        private const string AttackRightParam = "AttackRight";
        private const string AttackLeftParam = "AttackLeft";

        public static readonly int IdleDirection = Animator.StringToHash(IdleDirectionParam);
        public static readonly int WalkingRightPlayer = Animator.StringToHash(WalkRightPlayerParam);
        public static readonly int WalkingLeftPlayer = Animator.StringToHash(WalkLeftPlayerParam);
        public static readonly int WalkingEnemy = Animator.StringToHash(WalkEnemyParam);
        public static readonly int EnemyAttack = Animator.StringToHash(EnemyAttackParam);
        public static readonly int AttackRight = Animator.StringToHash(AttackRightParam);
        public static readonly int AttackLeft = Animator.StringToHash(AttackLeftParam);
    }
}