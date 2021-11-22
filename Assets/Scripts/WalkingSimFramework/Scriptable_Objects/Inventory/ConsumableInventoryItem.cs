using WalkingSimFramework.Helpers;
using WalkingSimFramework.Scriptable_Objects.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WalkingSimFramework.Scriptable_Objects.Inventory
{
    /// <summary>
    /// Item that can be used used/consumed
    /// </summary>
    [CreateAssetMenu(fileName = "Consumable Item", menuName = "WalkingSimFramework/Inventory/Consumable Item")]
    public class ConsumableInventoryItem : InventoryItemBase
    {
        [Tooltip("Amount of value the item has (ex: health, money, etc")]
        public int itemValue; 

        public override InventoryItemType GetItemType()
        {
            return InventoryItemType.CONSUMABLE;
        }
    }
}
