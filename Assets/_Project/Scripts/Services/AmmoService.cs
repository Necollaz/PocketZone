using System;
using UnityEngine;
using _Project.Scripts.Data;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Models.InventoryModel.Items;

namespace _Project.Scripts.Services
{
    public class AmmoService
    {
        private const string _saveFileName = "ammo_save.json";
        
        private readonly Ammo _ammo;
        private readonly IPersistenceService _persistence;

        public AmmoService(Ammo ammo, IPersistenceService persistence)
        {
            _ammo = ammo;
            _persistence = persistence;
            
            _ammo.AmmoCountChanged += count => AmmoCountChanged?.Invoke(count);
        }

        public event Action<int> AmmoCountChanged;
        
        public int GetCurrent() => _ammo.CurrentAmmo;
        
        public void Add(int amount) => _ammo.Add(amount);
        
        public void Use(int amount) => _ammo.Use(amount);

        public void Save()
        {
            AmmoSaveData data = new AmmoSaveData { CurrentAmmo = _ammo.CurrentAmmo };
            string json = JsonUtility.ToJson(data);
            
            _persistence.SaveToFile(_saveFileName, json);
        }

        public void Load()
        {
            if (!_persistence.FileExists(_saveFileName))
                return;
            
            string json = _persistence.LoadFromFile(_saveFileName);
            AmmoSaveData data = JsonUtility.FromJson<AmmoSaveData>(json);
            
            int diff = data.CurrentAmmo - _ammo.CurrentAmmo;
            
            if (diff > 0)
                _ammo.Add(diff);
            else if (diff < 0)
                _ammo.Use(-diff);
        }
    }
}