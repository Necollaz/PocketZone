using UnityEngine;
using Zenject;
using _Project.Scripts.Controllers;
using _Project.Scripts.Services;
using _Project.Scripts.Views;

namespace _Project.Scripts.Models.InventoryModel.Items
{
    [RequireComponent(typeof(Collider2D))]
    public class ItemPickup : MonoBehaviour
    {
        private const string BulletName = "Ammo";
        
        [SerializeField] private string _itemId;
        [SerializeField] private int _amount = 1;

        private InventoryService _inventoryService;
        private AmmoController _ammoController;

        [Inject]
        public void Construct(InventoryService inventoryService, AmmoController ammoController)
        {
            _inventoryService = inventoryService;
            _ammoController = ammoController;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            PlayerView playerView = other.GetComponentInParent<PlayerView>();
            
            if (playerView == null)
                return;

            if (_itemId == BulletName)
            {
                _ammoController.Add(_amount);
            }
            else
            {
                Sprite sprite = Resources.Load<Sprite>($"Icons/{_itemId}");
                
                if (sprite != null)
                {
                    Item item = new Item(_itemId, sprite, _amount);
                    
                    _inventoryService.AddItem(item);
                    _inventoryService.Save();
                }
            }

            Destroy(gameObject);
        }
    }
}