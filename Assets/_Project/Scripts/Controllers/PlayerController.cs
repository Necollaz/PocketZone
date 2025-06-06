using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Zenject;
using _Project.Scripts.Data.Configs;
using _Project.Scripts.Models;
using _Project.Scripts.Services;
using _Project.Scripts.Views;

namespace _Project.Scripts.Controllers
{
    public class PlayerController : ITickable
    {
        private readonly Player _player;
        private readonly PlayerView _view;
        private readonly HealthView _healthView;
        private readonly WeaponView _weaponView;
        private readonly InventoryView _inventoryView;
        private readonly InputService _inputService;
        private readonly PlayerConfig _config;
        private readonly AmmoService _ammoService;
        private readonly AmmoController _ammoController;

        private readonly int _bulletsPerShot = 1;

        private float _lastDirectionX = 1f;
        private float _lastShootTime = 0f;

        [Inject]
        public PlayerController(Player player, PlayerView view, HealthView healthView, WeaponView weaponView, InputService inputService, PlayerConfig config,
            InventoryView inventoryView, AmmoService ammoService, AmmoController ammoController)
        {
            _player = player;
            _view = view;
            _healthView = healthView;
            _weaponView = weaponView;
            _inputService = inputService;
            _config = config;
            _inventoryView = inventoryView;
            _ammoService = ammoService;
            _ammoController = ammoController;

            _player.Died += OnPlayerDied;
            _player.HealthChanged += OnHealthChanged;

            _healthView.Initialize(_player.CurrentHealth, _player.MaxHealth);
            _weaponView.InitializePool(_config.BulletPoolSize, _config.BulletSpeed, _config.BulletDamage, _config.BulletLifetime);
            _view.SetController(this);
        }

        public void Tick()
        {
            HandleMovement();
            HandleShooting();
        }

        public void TryShoot()
        {
            if (_ammoService.GetCurrent() <= 0)
                return;

            if (Time.time - _lastShootTime < _config.ShootCooldown)
                return;

            _lastShootTime = Time.time;

            Vector2 playerPosition = _view.transform.position;
            Vector2 aimDirection = DetermineAimDirection(playerPosition);

            UpdateLastDirection(aimDirection.x);

            _view.PlayAttack(_lastDirectionX);
            _weaponView.Shoot(aimDirection);

            for (int i = 0; i < _bulletsPerShot; i++)
                _ammoController.Use(1);
        }

        private void HandleMovement()
        {
            Vector2 move = _inputService.GetMovement();
            _player.MovementDirection = move;
            _player.IsWalking = move.sqrMagnitude > Mathf.Epsilon;

            _view.SetVelocity(move * _player.Speed);

            UpdateLastDirection(move.x);
            UpdateViewWalkDirection();
        }

        private void UpdateLastDirection(float moveX)
        {
            if (moveX > 0.01f)
                _lastDirectionX = 1f;
            else if (moveX < -0.01f)
                _lastDirectionX = -1f;
        }

        private void UpdateViewWalkDirection()
        {
            bool walkRight = false;
            bool walkLeft = false;

            if (_player.IsWalking)
            {
                float dirX = _player.MovementDirection.x;

                if (dirX > 0.01f)
                {
                    walkRight = true;
                }
                else if (dirX < -0.01f)
                {
                    walkLeft = true;
                }
                else
                {
                    if (_lastDirectionX > 0f)
                        walkRight = true;
                    else
                        walkLeft = true;
                }
            }

            _view.SetWalkDirection(walkRight, walkLeft, _lastDirectionX);
        }

        private void HandleShooting()
        {
            if (!_inputService.IsFirePressed() || _inventoryView.IsInventoryPanelActive || EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            TryShoot();
        }

        private Vector2 DetermineAimDirection(Vector2 playerPosition)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(playerPosition, _config.ShootRange, _config.EnemyLayerMask);
            
            if (hits.Length == 0)
            {
                return new Vector2(_lastDirectionX, 0f);
            }
            
            Collider2D closest = null;
            float minDistSq = float.MaxValue;

            foreach (var hit in hits)
            {
                Vector2 enemyPos = hit.transform.position;
                float distSq = (enemyPos - playerPosition).sqrMagnitude;
                
                if (distSq <= _config.ShootRange * _config.ShootRange && distSq < minDistSq)
                {
                    minDistSq = distSq;
                    closest   = hit;
                }
            }
            
            if (closest == null)
            {
                return new Vector2(_lastDirectionX, 0f);
            }
            
            Vector2 closestPos = closest.transform.position;
            return (closestPos - playerPosition).normalized;
        }

        private void OnHealthChanged(int newHealth)
        {
            _healthView.UpdateHealth(newHealth);
        }

        private void OnPlayerDied()
        {
            _view.gameObject.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}