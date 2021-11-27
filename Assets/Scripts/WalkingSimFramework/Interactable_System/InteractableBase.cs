using WalkingSimFramework.Helpers;
using WalkingSimFramework.Helpers.Highlight;
using WalkingSimFramework.Scriptable_Objects;
using WalkingSimFramework.Scriptable_Objects.Inventory;
using UnityEngine;

namespace WalkingSimFramework.Interactable_System
{
    /// <summary>
    /// Base class for interactable items
    /// </summary>
    public abstract class InteractableBase : MonoBehaviour, IInteractable
    {
        [Space, Header("Interactable Settings")]
        [Tooltip("Should the item be interactable")]
        [SerializeField] private bool isInteractable = true;
        [Tooltip("(Optional) External event(s) that occur when item is interacted with")]
        [SerializeField] private InteractionEvent itemInteractEvent;

        [Space, Header("Inventory Settings")]
        [Tooltip("(Optional) Type of inventory item that this object represents")]
        [SerializeField] protected InventoryItemBase inventoryItem = null;

        [Space, Header("Equipment Settings")]
        [SerializeField] private EquipmentEventInfo equipmentEventInfo;

        [Space, Header("Crosshair Hover Settings")]
        [SerializeField] private InteractableItemCrosshairHoverInfo itemCrosshairHoverInfo;

        [Space, Header("Interaction Data")]
        [Tooltip("Layer in the URP asset that us used for highlighing items. Note: Currently this only supports opaque meshes.")]
        [SerializeField] private HighlightItem highlightItem;
        [Tooltip("Data used to hold the current input action map")]
        [SerializeField] private InputActionMapData inputActionMapData = null;

        [Space, Header("Audio Settings")]
        [SerializeField] protected ItemSounds itemSounds;

        /// <summary>
        /// True if in the process of interacting with the item (i.e. examining an item)
        /// </summary>
        public bool IsCurrentlyInteracting { get; set; }

        public bool IsInteractable
        {
            get => isInteractable;
            set => isInteractable = value;
        }

        public InteractableItemCrosshairHoverInfo InteractableItemCrosshairHoverInfo => itemCrosshairHoverInfo;

        protected InputActionWrapper InpActionMap { get; set; }

        protected virtual void Awake()
        {
            itemSounds.InitSoundItem(gameObject);

            highlightItem?.InitInstance(gameObject);

            if(inputActionMapData != null)
            {
                InpActionMap = inputActionMapData.InputActionMap;
            }
        }

        public virtual void OnInteract()
        {
            SpawnEquipmentItem();
        }

        private void SpawnEquipmentItem()
        {
            if (IsCorrectItemEqipped())
            {
                if (equipmentEventInfo.EquipmentSpawnLocation != null && equipmentEventInfo.EquipmentSpawnItem != null)
                {
                    GameObject _equipment = Instantiate(equipmentEventInfo.EquipmentSpawnItem, equipmentEventInfo.EquipmentSpawnLocation.position, equipmentEventInfo.EquipmentSpawnLocation.rotation);
                    InteractableBase _interactableEquipment = _equipment.GetComponentInChildren<InteractableBase>();  

                    if (equipmentEventInfo.ShouldDisableItems)
                    {
                        if (_interactableEquipment != null)
                        {
                            _interactableEquipment.IsInteractable = !equipmentEventInfo.ShouldDisableItems;
                        }
                        IsInteractable = !equipmentEventInfo.ShouldDisableItems;
                    }
                }
            }
        }

        public bool IsCorrectItemEqipped()
        {
            return equipmentEventInfo.EquippedItemVar.isValid && equipmentEventInfo.EquipmentItemType == equipmentEventInfo.EquippedItemVar.value.ItemType;
        }

        public bool IsWrongItemEqipped()
        {
            if(!equipmentEventInfo.EquippedItemVar.isValid)
            {
                return false; // If have no equipment set, vacuously have correct item
            }

            bool _isEmpty = equipmentEventInfo.EquippedItemVar.value.ItemType == InventoryEquipmentTypes.NONE;

            return !_isEmpty && equipmentEventInfo.EquipmentItemType != equipmentEventInfo.EquippedItemVar.value.ItemType;
        }

        /// <summary>
        /// Fires the Equipment item used event if the correct item is equippped. 
        /// Separated from the base OnInteract method to give finer control over when it is called (subclasses decide).
        /// </summary>
        protected bool OnUseEquippedItem()
        {
            bool _isCorrectItemEquipped = IsCorrectItemEqipped();

            if (_isCorrectItemEquipped)
            {
                itemSounds.PlayEquipmentUsedSuccessfullySound();
                equipmentEventInfo.OnEquipmentItemUsedEvent.InvokeInteraction();
            }
            else
            {
                itemSounds.PlayEquipmentUsedUnSuccessfullySound();
            }

            return _isCorrectItemEquipped;
        }

        protected void OnItemInteractEvent()
        {
            itemInteractEvent.InvokeInteraction();
        }

        /// <summary>
        /// Destroys the item only after a sound clip is played, if available.
        /// Only the Pickup sound will be used.
        /// </summary>
        public void PlaySoundBeforeDestroy()
        {
            AudioSource _itemAudioSource = itemSounds.ItemAudioSource;

            if (_itemAudioSource != null)
            {
                _itemAudioSource.Play();
                _itemAudioSource.gameObject.transform.parent = null;  // Detach from parent

                float _timeToDestroy = itemSounds.PickUpSound == null ? 0f : itemSounds.PickUpSound.length;

                // Destroy the gameObject that plays the sound only when the sound has finished
                Destroy(_itemAudioSource.gameObject, _timeToDestroy);
            }

            // Destroy the parent right away
            Destroy(gameObject);
        }

        public InventoryItemBase GetInventorytItem()
        {
            return inventoryItem;
        }

        public void OnHoverOver()
        {
            if (!IsCurrentlyInteracting)
            {
                highlightItem?.OnItemHover();
            }
        }

        public void ResetAppearance()
        {
            highlightItem?.ResetHightlight();
        }
    }
}
