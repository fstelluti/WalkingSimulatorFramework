using WalkingSimFramework.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WalkingSimFramework.Scriptable_Objects.Inventory
{
    /// <summary>
    /// Item that can be equipped
    /// </summary>
    [CreateAssetMenu(fileName = "Equipable Item", menuName = "WalkingSimFramework/Inventory/Equipable Item")]
    public class EquipableInventoryItem : InventoryItemBase
    {
        public InventoryEquipmentTypes equipItemType;

        public override InventoryItemType GetItemType()
        {
            return InventoryItemType.EQUIPABLE;
        }
    }
}
