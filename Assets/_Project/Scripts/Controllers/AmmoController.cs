using UnityEngine;
using _Project.Scripts.Models.InventoryModel.Items;
using _Project.Scripts.Services;
using _Project.Scripts.Views;

namespace _Project.Scripts.Controllers
{
    public class AmmoController
    {
        private const string BulletName = "Ammo";
        private const string Path = "Icons/Ammo";

        private readonly AmmoService _ammoService;
        private readonly InventoryService _inventoryService;
        private readonly InventoryView _view;

        private bool _isInitialSync = false;

        public AmmoController(AmmoService ammoService, InventoryService inventoryService, InventoryView view)
        {
            _ammoService = ammoService;
            _inventoryService = inventoryService;
            _view = view;

            _ammoService.AmmoCountChanged += UpdateUI;
            _inventoryService.InventoryChanged += OnInventoryChangedAll;
            
            _ammoService.Load();

            UpdateUI(_ammoService.GetCurrent());
        }

        public void Add(int amount)
        {
            _ammoService.Add(amount);
            _ammoService.Save();

            Sprite ammoIcon = Resources.Load<Sprite>(Path);

            if (ammoIcon != null)
            {
                _inventoryService.AddItem(new Item(BulletName, ammoIcon, amount));
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

        private void UpdateUI(int currentAmmo)
        {
            _view.SetAmmoCount(currentAmmo);
        }

        private void OnInventoryChangedAll(Item[] slots)
        {
            if (!_isInitialSync)
            {
                _isInitialSync = true;
                
                Item ammoSlot = null;
                
                foreach (Item slot in slots)
                {
                    if (slot != null && slot.Id == BulletName)
                    {
                        ammoSlot = slot;
                        
                        break;
                    }
                }

                int currentAmmoModel = _ammoService.GetCurrent();
                int inventoryStack = ammoSlot != null ? ammoSlot.StackSize : 0;
                
                if (ammoSlot != null && inventoryStack != currentAmmoModel)
                {
                    int diff = inventoryStack - currentAmmoModel;
                    
                    if (diff > 0)
                        _ammoService.Add(diff);
                    else if (diff < 0)
                        _ammoService.Use(-diff);

                    _ammoService.Save();
                }
                else if (ammoSlot == null && currentAmmoModel > 0)
                {
                    Sprite ammoIcon = Resources.Load<Sprite>(Path);
                    
                    if (ammoIcon != null)
                    {
                        Item newAmmoItem = new Item(BulletName, ammoIcon, currentAmmoModel);
                        
                        _inventoryService.AddItem(newAmmoItem);
                        _inventoryService.Save();
                    }
                }
                
                UpdateUI(_ammoService.GetCurrent());
                
                return;
            }
            
            bool hasAmmo = false;
            
            foreach (Item slot in slots)
            {
                if (slot != null && slot.Id == BulletName)
                {
                    hasAmmo = true;
                    break;
                }
            }
            
            if (!hasAmmo)
            {
                int current = _ammoService.GetCurrent();
                
                if (current > 0)
                {
                    _ammoService.Use(current);
                    _ammoService.Save();
                }
            }
        }
    }
}