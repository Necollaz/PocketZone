using System;
using _Project.Scripts.Inventory.Model.Items;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace _Project.Scripts.InventoryComponents.View
{
    public class InventorySlotView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _countText;
        [SerializeField] private Button _iconButton;
        [SerializeField] private Button _removeButton;

        private int _slotIndex;

        public event Action<int> RemoveClicked;

        private void Awake()
        {
            _icon.enabled = false;
            _icon.sprite = null;
            _countText.gameObject.SetActive(false);
            _countText.text = "";
            _removeButton.gameObject.SetActive(false);
            _removeButton.onClick.AddListener(OnRemoveButtonClicked);
            _iconButton.onClick.AddListener(OnIconClicked);
        }

        private void OnDestroy()
        {
            _removeButton.onClick.RemoveListener(OnRemoveButtonClicked);
            _iconButton.onClick.RemoveListener(OnIconClicked);
        }

        public void SetData(int slotIndex, Item item)
        {
            _slotIndex = slotIndex;

            if (item == null)
            {
                _icon.enabled = false;
                _icon.sprite = null;
                _countText.gameObject.SetActive(false);
                _countText.text = "";
                _removeButton.gameObject.SetActive(false);
            }
            else
            {
                _icon.enabled = true;
                _icon.sprite = item.Icon;

                if (_countText != null)
                {
                    if (item.StackSize > 1)
                    {
                        _countText.text = item.StackSize.ToString();
                        _countText.gameObject.SetActive(true);
                    }
                    else
                    {
                        _countText.gameObject.SetActive(false);
                        _countText.text = "";
                    }
                }

                _removeButton.gameObject.SetActive(false);
            }
        }

        private void OnIconClicked()
        {
            if (_icon == null || !_icon.enabled)
                return;

            bool currentlyActive = _removeButton.gameObject.activeSelf;

            _removeButton.gameObject.SetActive(!currentlyActive);
        }

        private void OnRemoveButtonClicked()
        {
            RemoveClicked?.Invoke(_slotIndex);
        }
    }
}