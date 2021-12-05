using WalkingSimFramework.Scriptable_Objects.Inventory;
using System;
using UnityEngine;
using SmartData.SmartString.Data;

namespace WalkingSimFramework.Scriptable_Objects
{
    /// <summary>
    /// Demo class representing the data used when an item is equipped 
    /// </summary>
    [Serializable]
    public class EquipmentPanelData
    {
        private Sprite itemSprite;
        private string description;
        private StringConst itemType;

        public Sprite ItemSprite { get => itemSprite; set => itemSprite = value; }
        public string Description { get => description; set => description = value; }
        public StringConst ItemType { get => itemType; set => itemType = value; }

        public EquipmentPanelData()
        {
            description = "None";
            itemSprite = null;
            itemType = null;
        }

        public void ClearEquipmentPanelData()
        {
            description = "None";
            itemSprite = null;
            itemType = null;
        }

        public override string ToString()
        {
            return "Desc: " + Description + " ItemType: " + ItemType; 
        }
    }
}
