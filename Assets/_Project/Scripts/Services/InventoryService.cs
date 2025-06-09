using System;
using UnityEngine;
using _Project.Scripts.Data;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Models.InventoryModel;
using _Project.Scripts.Models.InventoryModel.Items;

namespace _Project.Scripts.Services
{
    public class InventoryService 
    {
        private const string SaveFileName = "inventory_save.json";

        private readonly Inventory _inventory;
        private readonly IPersistenceService _persistence;

        public InventoryService(Inventory inventory, IPersistenceService persistence)
        {
            _inventory = inventory;
            _persistence = persistence;
            
            _inventory.InventoryChanged += slots => InventoryChanged?.Invoke(slots);
        }
        
        public event Action<Item[]> InventoryChanged;

        public Item[] GetSlots() => _inventory.GetAllSlots();

        public void AddItem(Item item)
        {
            if (_inventory.TryAddItem(item))
                InventoryChanged?.Invoke(_inventory.GetAllSlots());
        }

        public void RemoveItem(int slotIndex)
        {
            _inventory.TryRemoveItem(slotIndex);
            
            InventoryChanged?.Invoke(_inventory.GetAllSlots());
        }
        
        public void DecreaseItem(string id, int amount)
        {
            Item[] slots = _inventory.GetAllSlots();
            
            for (int i = 0; i < slots.Length; i++)
            {
                Item slotItem = slots[i];
                
                if (slotItem != null && slotItem.Id == id)
                {
                    slotItem.Remove(amount);
                    
                    if (slotItem.StackSize <= 0)
                    {
                        _inventory.TryRemoveItem(i);
                    }
                    
                    InventoryChanged?.Invoke(_inventory.GetAllSlots());
                    
                    return;
                }
            }
        }
        
        public void Save()
        {
            InventorySaveData dataList = new InventorySaveData(_inventory.GetAllSlots());
            string json = JsonUtility.ToJson(dataList);
            
            _persistence.SaveToFile(SaveFileName, json);
        }
        
        public void Load()
        {
            if (!_persistence.FileExists(SaveFileName))
            {
                return;
            }
        
            string json = _persistence.LoadFromFile(SaveFileName);
            InventorySaveData inventorySaveData = JsonUtility.FromJson<InventorySaveData>(json);
            
            for (int i = 0; i < _inventory.MaxSlots; i++)
            {
                _inventory.TryRemoveItem(i);
            }
            
            for (int i = 0; i < inventorySaveData.Slots.Length; i++)
            {
                SlotData slotData = inventorySaveData.Slots[i];
                
                if (string.IsNullOrEmpty(slotData.Id))
                    continue;
            
                Sprite icon = Resources.Load<Sprite>("Icons/" + slotData.Id);
                
                if (icon == null)
                    continue;

                Item item = new Item(slotData.Id, icon, slotData.StackSize);
                
                _inventory.TryAddItem(item);
            }
            
            InventoryChanged?.Invoke(_inventory.GetAllSlots());
        }
    }
}