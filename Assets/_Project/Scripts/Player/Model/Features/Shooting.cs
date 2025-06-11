using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using _Project.Scripts.Data.Configs;
using _Project.Scripts.InventoryComponents.View;
using _Project.Scripts.Player.View;
using _Project.Scripts.PlayerInput;
using _Project.Scripts.Weapon.Controller;
using _Project.Scripts.Weapon.Model;
using _Project.Scripts.Weapon.View;

namespace _Project.Scripts.Player.Model.Features
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

        private readonly int _projectilePerShoot = 1;
        
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
                TryShoot();
        }
        
        private void OnAttackButtonPressed()
        {
            if (_inventoryView.IsInventoryPanelActive)
                return;
            
            TryShoot();
        }

        private void TryShoot()
        {
            if (_ammo.GetCurrent() <= 0 || Time.time - _lastShotTime < _config.ShootCooldown)
                return;

            Vector2 playerPosition = _view.transform.position;
            Vector2? targetDir = _aim.GetTargetDirection(playerPosition);
            
            if (!targetDir.HasValue)
                return;

            _lastShotTime = Time.time;
            Vector2 direction = targetDir.Value;

            _view.PlayAttack(Mathf.Sign(direction.x));
            _weapon.Shoot(direction);
            _ammoController.Use(_projectilePerShoot);
        }
    }
}