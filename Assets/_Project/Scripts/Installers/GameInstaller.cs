using UnityEngine;
using Zenject;
using _Project.Scripts.Controllers;
using _Project.Scripts.Data.Configs;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Models;
using _Project.Scripts.Models.InventoryModel;
using _Project.Scripts.Models.InventoryModel.Items;
using _Project.Scripts.SaveFuatures;
using _Project.Scripts.Services;
using _Project.Scripts.UI;
using _Project.Scripts.Views;

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
            Container.Bind<Player>().AsSingle().WithArguments(_playerConfig.Speed, _playerConfig.MaxHealth);

            Container.Bind<PlayerView>().FromInstance(_playerView).AsSingle();
            Container.Bind<HealthView>().FromInstance(_healthView).AsSingle();
            Container.Bind<WeaponView>().FromInstance(_weaponView).AsSingle();

            Container.Bind<IJoystick>().FromInstance(_joystick).AsSingle();
            Container.Bind<InputService>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<PlayerController>().AsSingle().NonLazy();
            
            Container.Bind<IPersistenceService>().To<FilePersistenceService>().AsSingle();
            
            Container.Bind<Inventory>().FromInstance(new Inventory(_maxSlotsInventory)).AsSingle();
            Container.Bind<InventoryService>().AsSingle();
            Container.Bind<InventoryView>().FromInstance(_inventoryView).AsSingle();
            Container.BindInterfacesAndSelfTo<InventoryController>().AsSingle().NonLazy();
            
            Container.Bind<Ammo>().FromInstance(new Ammo(_initialAmmoCount)).AsSingle();
            Container.Bind<AmmoService>().AsSingle();
            Container.BindInterfacesAndSelfTo<AmmoController>().AsSingle().NonLazy();
        }
    }
}