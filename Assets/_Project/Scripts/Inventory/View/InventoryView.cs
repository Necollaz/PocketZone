using System;
using _Project.Scripts.Inventory.Model.Items;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace _Project.Scripts.InventoryComponents.View
{
    public class InventoryView : MonoBehaviour
    {
        [Header("Inventory UI")]
        [SerializeField] private Button _toggleInventoryButton;
        [SerializeField] private GameObject _inventoryPanel;
        [SerializeField] private RectTransform _slotsParent;
        [SerializeField] private InventorySlotView _slotPrefab;

        [Header("Ammo UI")]
        [SerializeField] private TextMeshProUGUI _ammoText;

        public bool IsInventoryPanelActive => _inventoryPanel.activeSelf;

        public event Action ToggleInventory;
        public event Action<int> SlotRemoveRequested;

        private void Awake()
        {
            _inventoryPanel.SetActive(false);
            _toggleInventoryButton.onClick.AddListener(OnToggleClicked);
        }

        private void OnDestroy()
        {
            _toggleInventoryButton.onClick.RemoveListener(OnToggleClicked);
        }

        public void RebuildSlots(Item[] slots)
        {
            foreach (Transform child in _slotsParent)
                Destroy(child.gameObject);

            for (int i = 0; i < slots.Length; i++)
            {
                InventorySlotView slotView = Instantiate(_slotPrefab, _slotsParent);
                slotView.SetData(i, slots[i]);

                slotView.RemoveClicked += index => SlotRemoveRequested?.Invoke(index);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(_slotsParent);
        }

        public void SetInventoryPanelActive(bool isVisible)
        {
            _inventoryPanel.SetActive(isVisible);

            if (isVisible)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(_slotsParent);
            }
        }

        public void SetAmmoCount(int count)
        {
            if (_ammoText != null)
            {
                _ammoText.text = $"{count}";
            }
        }
        
        private void OnToggleClicked()
        {
            ToggleInventory?.Invoke();
        }
    }
}