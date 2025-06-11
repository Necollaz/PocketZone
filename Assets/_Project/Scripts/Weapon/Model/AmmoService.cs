using System;
using UnityEngine;
using _Project.Scripts.Common.Interfaces;
using _Project.Scripts.Data;

namespace _Project.Scripts.Weapon.Model
{
    public class AmmoService
    {
        private const string _saveFileName = "ammo_save.json";
        
        private readonly Ammo _ammo;
        private readonly IDataStorageService _dataStorage;

        public AmmoService(Ammo ammo, IDataStorageService dataStorage)
        {
            _ammo = ammo;
            _dataStorage = dataStorage;
            
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
            
            _dataStorage.SaveToFile(_saveFileName, json);
        }
    }
}