using System;
using _Project.Scripts.Models.InventoryModel.Items;

namespace _Project.Scripts.Data
{
    [Serializable]
    public struct InventorySaveData
    {
        public SlotData[] Slots;

        public InventorySaveData(Item[] slots)
        {
            Slots = new SlotData[slots.Length];
            
            for (int i = 0; i < slots.Length; i++)
            {
                Item item = slots[i];
                
                if (item != null)
                {
                    Slots[i] = new SlotData
                    {
                        Id = item.Id,
                        StackSize = item.StackSize
                    };
                }
                else
                {
                    Slots[i] = default;
                }
            }
        }
    }
}