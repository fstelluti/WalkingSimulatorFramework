using WalkingSimFramework.Scriptable_Objects.Inventory;
using NaughtyAttributes;
using SmartData.SmartEquippedPanelData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WalkingSimFramework.Helpers
{
    /// <summary>
    /// Groups together all equipment item event related information
    /// </summary>
    [System.Serializable]
    public class EquipmentEventInfo
    {
        [Tooltip("Equiptment type that can be used with this item (ex: key)")]
        [SerializeField] private InventoryEquipmentTypes equipmentItemType;
        [Tooltip("Equipped Item data")]
        [SerializeField] private EquippedPanelDataReader equippedItemVar;
        [Tooltip("(Optional) Location to spawn equipment item when used")]
        [SerializeField] private Transform equipmentSpawnLocation;
        [Tooltip("(Optional) Item to be spawned at the equipment spawn location. Warning: Needs to be a prefab that isn't in the scene so it will not be destroyed.")]
        [EnableIf("HasSpawnLocation")]
        [AllowNesting]
        [SerializeField] private GameObject equipmentSpawnItem;
        [Tooltip("(Optional) If current item and spawned item are disabled upon use (can no longer be interacted with)")]
        [EnableIf("HasSpawnLocation")]
        [AllowNesting]
        [SerializeField] private bool shouldDisableItems = true;
        [Tooltip("(Optional) Event that is called when the correct equipment is used on this item")]
        [SerializeField] private InteractionEvent onEquipmentItemUsedEvent;

        public bool HasSpawnLocation => equipmentSpawnLocation != null;

        public InventoryEquipmentTypes EquipmentItemType => equipmentItemType;

        public EquippedPanelDataReader EquippedItemVar => equippedItemVar;

        public Transform EquipmentSpawnLocation => equipmentSpawnLocation;

        public GameObject EquipmentSpawnItem => equipmentSpawnItem;

        public bool ShouldDisableItems => shouldDisableItems;

        public InteractionEvent OnEquipmentItemUsedEvent => onEquipmentItemUsedEvent;

    }
}
