using _Project.Scripts.Inventory.Model;
using _Project.Scripts.Inventory.Model.Items;
using UnityEngine;
using Zenject;
using _Project.Scripts.Weapon.Model;

namespace _Project.Scripts.Weapon.Controller
{
    public class AmmoController
    {
        private const string BulletName = "Ammo";
        private const string IconPath = "Icons/Ammo";

        private readonly AmmoService _ammoService;
        private readonly InventoryService _inventoryService;

        [Inject]
        public AmmoController(AmmoService ammoService, InventoryService inventoryService)
        {
            _ammoService = ammoService;
            _inventoryService = inventoryService;
        }

        public void Add(int amount)
        {
            _ammoService.Add(amount);
            _ammoService.Save();

            Sprite icon = Resources.Load<Sprite>(IconPath);
            
            if (icon != null)
            {
                _inventoryService.AddItem(new Item(BulletName, icon, amount));
                _inventoryService.Save();
            }
        }

        public void Use(int amount)
        {
            _ammoService.Use(amount);
            _ammoService.Save();

            _inventoryService.DecreaseItem(BulletName, amount);
            _inventoryService.Save();
        }
    }
}