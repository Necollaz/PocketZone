using UnityEngine;

namespace _Project.Scripts.Inventory.Model.Items
{
    public class Item
    {
        public string Id { get; }
        public Sprite Icon { get; }
        public int StackSize { get; private set; }

        public Item(string id, Sprite icon, int initialCount = 1)
        {
            Id = id;
            Icon = icon;
            StackSize = initialCount;
        }

        public void Add(int amount = 1)
        {
            StackSize += amount;
        }

        public void Remove(int amount = 1)
        {
            StackSize = Mathf.Max(0, StackSize - amount);
        }
    }
}