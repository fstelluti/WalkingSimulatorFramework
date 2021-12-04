using WalkingSimFramework.Scriptable_Objects.Inventory;
using System;
using UnityEngine;

namespace WalkingSimFramework.Scriptable_Objects
{
    /// <summary>
    /// Demo class representing the data used in the Equip item panel of the inventory
    /// </summary>
    [Serializable]
    public class EquippedPanelData
    {
        private Sprite itemSprite;
        private string description;
        private InventoryEquipmentTypes itemType;

        public Sprite ItemSprite { get => itemSprite; set => itemSprite = value; }
        public string Description { get => description; set => description = value; }
        public InventoryEquipmentTypes ItemType { get => itemType; set => itemType = value; }

        readonly static EquippedPanelData m_emptyEqipmentData = new EquippedPanelData();

        public static EquippedPanelData emptyEqipmentData => m_emptyEqipmentData;

        public EquippedPanelData()
        {
            Description = "None";
            ItemSprite = null;
            ItemType = InventoryEquipmentTypes.NONE;
        }
    }
}
