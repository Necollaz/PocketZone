using System;
using System.Linq;
using UnityEngine;
using Zenject;
using _Project.Scripts.Inventory.Model.Items;
using _Project.Scripts.InventoryComponents.View;
using _Project.Scripts.Weapon.Model;

namespace _Project.Scripts.Inventory.Model
{
    public class InventoryAmmoSyncService : IInitializable, IDisposable
    {
        private const string BulletName = "Ammo";
        private const string IconPath = "Icons/Ammo";
        
        private readonly AmmoService _ammoService;
        private readonly InventoryService _inventoryService;
        private readonly InventoryView _view;
        
        private bool _hasDoneInitialSync = false;

        [Inject]
        public InventoryAmmoSyncService(AmmoService ammoService, InventoryService inventoryService, InventoryView view)
        {
            _ammoService = ammoService;
            _inventoryService = inventoryService;
            _view = view;
            
            _ammoService.AmmoCountChanged += UpdateView;
            _inventoryService.InventoryChanged += OnInventoryChanged;
        }
        
        public void Initialize()
        {
            UpdateView(_ammoService.GetCurrent());
        }
        
        public void Dispose()
        {
            _ammoService.AmmoCountChanged -= UpdateView;
            _inventoryService.InventoryChanged -= OnInventoryChanged;
        }

        private void UpdateView(int currentAmmo)
        {
            _view.SetAmmoCount(currentAmmo);
        }

        private void OnInventoryChanged(Item[] slots)
        {
            if (!_hasDoneInitialSync)
            {
                DoInitialSync(slots);
                _hasDoneInitialSync = true;
            }
            else
            {
                DoRuntimeCleanup(slots);
            }
        }

        private void DoInitialSync(Item[] slots)
        {
            Item slot = slots.FirstOrDefault(i => i != null && i.Id == BulletName);
            int modelCount = _ammoService.GetCurrent();
            int invCount = slot?.StackSize ?? 0;

            if (slot != null && invCount != modelCount)
            {
                int diff = invCount - modelCount;
                
                if (diff > 0)
                    _ammoService.Add(diff);
                else
                    _ammoService.Use(-diff);
                
                _ammoService.Save();
            }
            else if (slot == null && modelCount > 0)
            {
                Sprite icon = Resources.Load<Sprite>(IconPath);
                
                if (icon != null)
                {
                    _inventoryService.AddItem(new Item(BulletName, icon, modelCount));
                    _inventoryService.Save();
                }
            }

            UpdateView(_ammoService.GetCurrent());
        }

        private void DoRuntimeCleanup(Item[] slots)
        {
            bool hasAmmo = slots.Any(item => item != null && item.Id == BulletName);
            
            if (!hasAmmo && _ammoService.GetCurrent() > 0)
            {
                _ammoService.Use(_ammoService.GetCurrent());
                _ammoService.Save();
            }
        }
    }
}