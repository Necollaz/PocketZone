using System;
using UnityEngine;

namespace _Project.Scripts.Models.InventoryModel.Items
{
    public class Ammo
    {
        private int _currentAmmo;

        public Ammo(int initialAmmo)
        {
            _currentAmmo = initialAmmo;
        }
        
        public int CurrentAmmo
        {
            get => _currentAmmo;
            private set
            {
                _currentAmmo = Mathf.Max(0, value);
                
                AmmoCountChanged?.Invoke(_currentAmmo);
            }
        }
        
        public event Action<int> AmmoCountChanged;

        public void Add(int amount)
        {
            CurrentAmmo += amount;
        }

        public void Use(int amount)
        {
            CurrentAmmo = Mathf.Max(0, CurrentAmmo - amount);
        }
    }
}