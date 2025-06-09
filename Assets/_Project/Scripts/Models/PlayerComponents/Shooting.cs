using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using _Project.Scripts.Controllers;
using _Project.Scripts.Data.Configs;
using _Project.Scripts.Services;
using _Project.Scripts.Views;

namespace _Project.Scripts.Models.PlayerComponents
{
    public class Shooting : ITickable, IInitializable, IDisposable
    {
        private readonly PlayerView _view;
        private readonly WeaponView _weapon;
        private readonly InputService _input;
        private readonly PlayerMovement _movement;
        private readonly AimCalculator _aim;
        private readonly PlayerConfig _config;
        private readonly AmmoService _ammo;
        private readonly AmmoController _ammoController;
        private readonly InventoryView _inventoryView;

        private int _projectilePerShoot = 1;
        private float _lastShotTime;

        [Inject]
        public Shooting(PlayerView view, WeaponView weapon, InputService input, PlayerMovement movement, AimCalculator aim, PlayerConfig config,
            AmmoService ammo, AmmoController ammoController, InventoryView inventoryView)
        {
            _view = view;
            _weapon = weapon;
            _input = input;
            _movement = movement;
            _aim = aim;
            _config = config;
            _ammo = ammo;
            _ammoController = ammoController;
            _inventoryView  = inventoryView;
            
            _weapon.InitializePool(config.BulletPoolSize, config.BulletSpeed, config.BulletDamage, config.BulletLifetime);
        }

        public void Initialize()
        {
            _view.AttackPressed += OnAttackButtonPressed;
        }
        
        public void Dispose()
        {
            _view.AttackPressed -= OnAttackButtonPressed;
            
        }
        public void Tick()
        {
            if (_inventoryView.IsInventoryPanelActive)
                return;
            
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;
            
            if (_input.IsFirePressed())
                TryShoot(useMovementDirection: false);
        }
        
        private void OnAttackButtonPressed()
        {
            if (_inventoryView.IsInventoryPanelActive)
                return;
            
            TryShoot(useMovementDirection: true);
        }

        private void TryShoot(bool useMovementDirection)
        {
            if (_ammo.GetCurrent() <= 0 || Time.time - _lastShotTime < _config.ShootCooldown)
                return;

            _lastShotTime = Time.time;
            Vector2 direction;

            if (useMovementDirection)
            {
                direction = new Vector2(_movement.LastDirectionX, 0f);
            }
            else
            {
                Vector3 position = _view.transform.position;
                direction = _aim.DetermineAimDirection(position, _movement.LastDirectionX);
            }

            _movement.LastDirectionX = direction.x != 0 ? Mathf.Sign(direction.x) : _movement.LastDirectionX;
            
            _view.PlayAttack(_movement.LastDirectionX);
            _weapon.Shoot(direction);
            _ammoController.Use(_projectilePerShoot);
        }
    }
}