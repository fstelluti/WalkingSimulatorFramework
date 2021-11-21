using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WalkingSimFramework.Scriptable_Objects.Inventory
{
    /// <summary>
    /// Enum representing the equip item type.
    /// Needed to be able to use items in the game.
    /// </summary>
    public enum InventoryEquipmentTypes
    {
        // TODO: Find a way to make this more intelligent (separate equippable from non-equippable types, read from a file?, etc). 

        // Add new equippable item types here if needed
        // Specify the index to be clearer and use a specific name
        NONE = 0,

        // Non-equippable 
        KEY_DESK_DRAWER = 1,
        KEY_DOOR_01 = 2,

        //Equippable (via UI)
        BATTERY_01 = 3
    }
}
