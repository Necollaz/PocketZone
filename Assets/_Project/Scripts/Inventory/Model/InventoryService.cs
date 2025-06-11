using System;
using UnityEngine;
using _Project.Scripts.Common.Interfaces;
using _Project.Scripts.Data;
using _Project.Scripts.Inventory.Model.Items;

namespace _Project.Scripts.Inventory.Model
{
    public class InventoryService 
    {
        private const string SaveFileName = "inventory_save.json";

        private readonly InventoryModel _inventoryModel;
        private readonly IDataStorageService _dataStorage;

        public InventoryService(InventoryModel inventoryModel, IDataStorageService dataStorage)
        {
            _inventoryModel = inventoryModel;
            _dataStorage = dataStorage;
            
            _inventoryModel.InventoryChanged += slots => InventoryChanged?.Invoke(slots);
        }
        
        public event Action<Item[]> InventoryChanged;

        public Item[] GetSlots() => _inventoryModel.GetAllSlots();

        public void AddItem(Item item)
        {
            if (_inventoryModel.TryAddItem(item))
                InventoryChanged?.Invoke(_inventoryModel.GetAllSlots());
        }

        public void RemoveItem(int slotIndex)
        {
            _inventoryModel.TryRemoveItem(slotIndex);
            
            InventoryChanged?.Invoke(_inventoryModel.GetAllSlots());
        }
        
        public void DecreaseItem(string id, int amount)
        {
            Item[] slots = _inventoryModel.GetAllSlots();
            
            for (int i = 0; i < slots.Length; i++)
            {
                Item slotItem = slots[i];
                
                if (slotItem != null && slotItem.Id == id)
                {
                    slotItem.Remove(amount);
                    
                    if (slotItem.StackSize <= 0)
                    {
                        _inventoryModel.TryRemoveItem(i);
                    }
                    
                    InventoryChanged?.Invoke(_inventoryModel.GetAllSlots());
                    
                    return;
                }
            }
        }
        
        public void Save()
        {
            InventorySaveData dataList = new InventorySaveData(_inventoryModel.GetAllSlots());
            string json = JsonUtility.ToJson(dataList);
            
            _dataStorage.SaveToFile(SaveFileName, json);
        }
        
        public void Load()
        {
            if (!_dataStorage.FileExists(SaveFileName))
            {
                return;
            }
        
            string json = _dataStorage.LoadFromFile(SaveFileName);
            InventorySaveData inventorySaveData = JsonUtility.FromJson<InventorySaveData>(json);
            
            for (int i = 0; i < _inventoryModel.MaxSlots; i++)
            {
                _inventoryModel.TryRemoveItem(i);
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
                
                _inventoryModel.TryAddItem(item);
            }
            
            InventoryChanged?.Invoke(_inventoryModel.GetAllSlots());
        }
    }
}