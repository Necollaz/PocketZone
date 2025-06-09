using UnityEngine;
using Zenject;
using _Project.Scripts.Models.EnemyModel.Features;
using _Project.Scripts.Models.PlayerComponents;
using _Project.Scripts.Services;
using _Project.Scripts.Views;

namespace _Project.Scripts.Models.EnemyModel
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyAI : MonoBehaviour
    {
        [Header("Attack Settings")]
        [SerializeField] private int _attackDamage = 10;
        [SerializeField] private float _attackCooldown = 1f;
        [SerializeField] private float _attackDistance = 1f;

        [Header("Detection & Movement")]
        [SerializeField] private float _detectionRadius = 5f;
        [SerializeField] private float _moveSpeed = 2f;

        private Rigidbody2D _rigidbody2D;
        private Animator _animator;

        private EnemyAIService _enemyAIService;
        private Transform _playerTransform;
        private Player _player;

        [Inject]
        public void Construct(PlayerView playerView, Player player)
        {
            _playerTransform = playerView.transform;
            _player = player;
        }

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            
            EnemyRotation rotation = new EnemyRotation();
            EnemyMovement movement = new EnemyMovement(_moveSpeed, transform.position);
            EnemyAttack attack = new EnemyAttack(_attackDamage, _attackCooldown, _attackDistance);
            
            _enemyAIService = new EnemyAIService(rotation, movement, attack, _detectionRadius);
        }

        private void Update()
        {
            _enemyAIService.Update(transform, _rigidbody2D, _animator, _playerTransform, _player);
        }
        
        public void OnDealDamage()
        {
            _enemyAIService.OnDealDamage(transform, _playerTransform, _player);
        }

        public void OnAttackEnded()
        {
            _enemyAIService.OnAttackEnded();
        }
    }
}