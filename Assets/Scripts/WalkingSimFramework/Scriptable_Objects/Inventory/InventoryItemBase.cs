using WalkingSimFramework.Helpers;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace WalkingSimFramework.Scriptable_Objects.Inventory
{
    /// <summary>
    /// Base class that represents an inventory item
    /// </summary>
    public abstract class InventoryItemBase : ScriptableObject
    {
        [Tooltip("ID for the item (should be unique)")]
        public string id;

        [Tooltip("Sprite for the UI")]
        public Sprite itemSprite;

        [Tooltip("If item should be destroyed right away after selecting it")]
        public bool shouldDestroyImmediately;

        [Tooltip("If item should be used right away after selecting it")]
        public bool autoUse;

        [Tooltip("Is the item stackable on the same slot")]
        public bool isStackable;

        [Tooltip("Maximum amount of item that can be held in a slot")]
        [EnableIf("isStackable")]
        [MinValue(1)]
        public int maxStacks = 1;

        [Tooltip("Text to describe item")]
        public string shortDescription;

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            string _path = AssetDatabase.GetAssetPath(this);
            id = AssetDatabase.AssetPathToGUID(_path);
        }
#endif
        public abstract InventoryItemType GetItemType(); 
    }
}
