using WalkingSimFramework.Helpers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace WalkingSimFramework.Scriptable_Objects.Inventory
{
    /// <summary>
    /// Item that can contain text to be viewed 
    /// </summary>
    [CreateAssetMenu(fileName = "Note Item", menuName = "My Interaction System/Inventory/Note Item")]
    public class NoteInventoryItem : InventoryItemBase
    {
        [TextArea(3,20)]
        public string noteDescription; 

        public override InventoryItemType GetItemType()
        {
            return InventoryItemType.NOTE;
        }
    }
}
