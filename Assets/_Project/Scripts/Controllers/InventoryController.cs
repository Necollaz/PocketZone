using _Project.Scripts.Models.InventoryModel.Items;
using _Project.Scripts.Services;
using _Project.Scripts.Views;

namespace _Project.Scripts.Controllers
{
    public class InventoryController
    {
        private readonly InventoryService _inventoryService;
        private readonly InventoryView _view;

        public InventoryController(InventoryService inventoryService, InventoryView view)
        {
            _inventoryService = inventoryService;
            _view = view;
            
            _view.ToggleInventory += ToggleInventoryUI;
            _view.SlotRemoveRequested += RemoveItemAtSlot;
            _inventoryService.InventoryChanged += UpdateUI;
            
            _inventoryService.Load();

            if (_view.IsInventoryPanelActive)
            {
                _view.RebuildSlots(_inventoryService.GetSlots());
            }
        }
        
        private void ToggleInventoryUI()
        {
            bool newActiveState = !_view.IsInventoryPanelActive;
            
            _view.SetInventoryPanelActive(newActiveState);
            
            if (newActiveState)
            {
                Item[] currentSlots = _inventoryService.GetSlots();
                _view.RebuildSlots(currentSlots);
            }
        }

        private void RemoveItemAtSlot(int slotIndex)
        {
            _inventoryService.RemoveItem(slotIndex);
            _inventoryService.Save();
        }

        private void UpdateUI(Item[] slots)
        {
            if (_view.IsInventoryPanelActive)
            {
                _view.RebuildSlots(slots);
            }
        }
    }
}