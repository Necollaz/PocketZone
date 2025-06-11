using UnityEngine;
using Zenject;
using _Project.Scripts.Common;
using _Project.Scripts.Common.Interfaces;
using _Project.Scripts.Data.Configs;
using _Project.Scripts.Inventory.Controller;
using _Project.Scripts.Inventory.Model;
using _Project.Scripts.InventoryComponents.View;
using _Project.Scripts.Player.Model;
using _Project.Scripts.Player.Model.Features;
using _Project.Scripts.Player.View;
using _Project.Scripts.PlayerInput;
using _Project.Scripts.PlayerInput.View;
using _Project.Scripts.Weapon.Controller;
using _Project.Scripts.Weapon.Model;
using _Project.Scripts.Weapon.View;

namespace _Project.Scripts.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private Camera _mainCamera;
        
        [Header("Player & Core Views")]
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private HealthView _healthView;
        [SerializeField] private WeaponView _weaponView;
        [SerializeField] private SimpleJoystick _joystick;
        [SerializeField] private PlayerConfig _playerConfig;
        
        [Header("Inventory UI Reference")]
        [SerializeField] private InventoryView _inventoryView;
        [SerializeField] private int _maxSlotsInventory = 5;
        [SerializeField] private int _initialAmmoCount = 30;
        
        public override void InstallBindings()
        {
            Container.Bind<Camera>().FromInstance(_mainCamera).AsSingle();
            
            Container.Bind<PlayerConfig>().FromInstance(_playerConfig).AsSingle();
            Container.Bind<PlayerModel>().AsSingle().WithArguments(_playerConfig.Speed, _playerConfig.MaxHealth);

            Container.Bind<PlayerView>().FromInstance(_playerView).AsSingle();
            Container.Bind<HealthView>().FromInstance(_healthView).AsSingle();
            Container.Bind<WeaponView>().FromInstance(_weaponView).AsSingle();
            Container.Bind<InventoryView>().FromInstance(_inventoryView).AsSingle();
            
            Container.Bind<IJoystick>().FromInstance(_joystick).AsSingle();
            Container.Bind<InputService>().AsSingle();
            Container.Bind<IDataStorageService>().To<FileDataStorageService>().AsSingle();
            
            Container.Bind<InventoryModel>().FromInstance(new InventoryModel(_maxSlotsInventory)).AsSingle();
            Container.Bind<InventoryService>().AsSingle();
            
            Container.Bind<Ammo>().FromInstance(new Ammo(_initialAmmoCount)).AsSingle();
            Container.Bind<AmmoService>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<PlayerMovement>().AsSingle().NonLazy();
            Container.Bind<AimCalculator>().AsSingle();
            Container.BindInterfacesAndSelfTo<Shooting>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PlayerHealth>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<DeathHandler>().AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<InventoryController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<AmmoController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<InventoryAmmoSyncService>().AsSingle().NonLazy();
        }
    }
}