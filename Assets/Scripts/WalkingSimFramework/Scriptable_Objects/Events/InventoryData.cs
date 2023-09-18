using WalkingSimFramework.Scriptable_Objects.Inventory;
using System.Collections.Generic;
using UnityEngine;

namespace WalkingSimFramework.Scriptable_Objects.Events
{
    /// <summary>
    /// Represents the data for a player's inventory system.
    /// Can be used to have different initial starting inventories
    /// </summary>
    [CreateAssetMenu(fileName = "Inventory Data", menuName = "WalkingSimFramework/Inventory/Inventory Data")]
    public class InventoryData : ScriptableObject 
    {
        // Caller's responsibility to subscribe/unsubscribe
        public delegate bool OnAddToInventoryDelegate(InventoryItemBase item);
        public OnAddToInventoryDelegate addToInventoryDelegate;

        public delegate void OnRemoveEqipmentFromInventoryDelegate();
        public OnRemoveEqipmentFromInventoryDelegate removeEqipmentFromInventoryDelegate;

        [SerializeField] private List<InventoryItemBase> inventoryItems;

        [Tooltip("Crosshair messages for the inventory")]
        [SerializeField] private string inventoryFull = "Inventory full";
        [SerializeField] private string wrongEquippedItem = "Cannot use this item here";

        public string InventoryFullMsg => inventoryFull;
        public string WrongEquippedItemMsg => wrongEquippedItem;

        private void OnDisable()
        {
#if UNITY_EDITOR
            // Use this to clear in the Editor only. 
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            {
                ClearIntenvory();
            }
#endif
        }

        /// <summary>
        /// Adds item to inventory UI and list
        /// </summary>
        public bool OnAddToInventory(InventoryItemBase _item) 
        {
            // Add to UI first
            bool _addedInventory = addToInventoryDelegate != null ? addToInventoryDelegate(_item) : false;

            if (_addedInventory)
            {
                AddItem(_item);
            }

            return _addedInventory;
        }

        public void OnRemoveEqippedItemFromInventory()
        {
            if (removeEqipmentFromInventoryDelegate != null)
            {
                removeEqipmentFromInventoryDelegate();
            }
        }

        public void OnRemoveFromInventory(InventoryItemBase _item)
        {
            RemoveItem(_item);
        }

        private void ClearIntenvory()
        {
            inventoryItems.Clear();
        }

        private bool AddItem(InventoryItemBase _item)
        {
            bool _containsItem = false;

            foreach(InventoryItemBase _itemBase in inventoryItems)
            {
                if(_itemBase.id.Equals(_item.id))
                {
                    _containsItem = true;
                    break;
                }
            }

            if (!_containsItem)
            {
                inventoryItems.Add(_item);
                return true;
            }

            return false;
        }

        private void RemoveItem(InventoryItemBase _item)
        {
            InventoryItemBase _removedItem = null;

            foreach (InventoryItemBase _itemBase in inventoryItems)
            {
                if (_itemBase.id.Equals(_item.id))
                {
                    _removedItem = _itemBase;
                    break;
                }
            }

            if (_removedItem != null)
            {
                inventoryItems.Remove(_removedItem);
            }
        }
    }
}
