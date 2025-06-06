using System;
using _Project.Scripts.Models.InventoryModel.Items;

namespace _Project.Scripts.Models.InventoryModel
{
    public class Inventory
    {
        private readonly Item[] _slots;

        public Inventory(int maxSlots)
        {
            MaxSlots = maxSlots;
            _slots = new Item[MaxSlots];
        }
        
        public int MaxSlots { get; }
        
        public event Action<Item[]> InventoryChanged;
        
        public Item[] GetAllSlots()
        {
            return _slots;
        }
        
        public bool AddItem(Item newItem)
        {
            for (int i = 0; i < MaxSlots; i++)
            {
                Item slot = _slots[i];
                
                if (slot != null && slot.Id == newItem.Id)
                {
                    slot.Add(newItem.StackSize);
                    
                    InventoryChanged?.Invoke(_slots);
                    
                    return true;
                }
            }
            
            for (int i = 0; i < MaxSlots; i++)
            {
                if (_slots[i] == null)
                {
                    _slots[i] = new Item(newItem.Id, newItem.Icon, newItem.StackSize);
                    
                    InventoryChanged?.Invoke(_slots);
                    
                    return true;
                }
            }
            
            return false;
        }
        
        public bool RemoveItem(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= MaxSlots)
                return false;
            
            if (_slots[slotIndex] == null)
                return false;

            _slots[slotIndex] = null;
            
            InventoryChanged?.Invoke(_slots);
            
            return true;
        }
    }
}