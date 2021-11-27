using WalkingSimFramework.Helpers;
using WalkingSimFramework.Scriptable_Objects;
using WalkingSimFramework.Scriptable_Objects.Events;
using WalkingSimFramework.Scriptable_Objects.Inventory;
using WalkingSimFramework.UI_System;
using NaughtyAttributes;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace WalkingSimFramework.Interactable_System
{
    /// <summary>
    /// Controller for any item that is interactable (selectable, collectable, etc).
    /// Contains scriptable object data references, which store the data based on the currrent object's values.
    /// </summary>
    public class InteractionController : MonoBehaviour
    {
        [Space, Header("Ray cast Settings")]
        [Tooltip("Max distance that item can be interacted with")]
        [SerializeField] private float rayDistance = 3f;
        [SerializeField] private float raySphereRadius = 0.1f;
        [Tooltip("Which layers can be interaacted with. Deselect layers to be able to ignore certain objects (i.e. to interact with " +
            "something through another object. Warning: FPS Controller needs to be in a layer that isn't in this list, else will hit itself and do nothing.")]
        [SerializeField] private LayerMask interactabletableLayers = ~0;

        [Space, Header("Crosshair")]
        [Tooltip("The script that manages the crosshair UI")]
        [SerializeField] private Transform crosshairUI;

        [Space, Header("Interaction Data")]
        [Tooltip("Data used to manage interactable objects")]
        [SerializeField] private InteractionableData interactionableData = null;
        [Tooltip("Data used to hold the current input action map")]
        [SerializeField] private InputActionMapData inputActionMapData = null;

        [Space, Header("Inventory Settings")]
        [SerializeField] private InventoryData inventoryData = null;

        [Space, Header("Proximity Highlight Setings")]
        [Tooltip("If should highlight items based on proximity, otherwise use per item highlight.")]
        [SerializeField] private bool useProximityItemHighlight;
        [Tooltip("Which layers can be highlighted for selection. Certain items might not need this, but can still be interacted with, for example doors.")]
        [SerializeField] private LayerMask highlightableItemLayers = 0;
        [Tooltip("Proximity highlight range")]
        [EnableIf("useProximityItemHighlight")]
        [SerializeField] private float proximityRangeRadius = 2.0f;
        [Tooltip("Max number of highlighted items at a given time")]
        [EnableIf("useProximityItemHighlight")]
        [SerializeField] private int maxNumberHighlighedItems = 20;

        IProximityHighlight m_proximityItemHighlight;

        Camera m_player_cam;

        InteractionUIPanel m_crosshairUI;

        InteractableItemCrosshairHoverInfo m_inventoryHoverInfo;

        // New Input System map
        InputActionWrapper InputActionMap;

        void Awake()
        {
            m_player_cam = FindObjectOfType<Camera>();

            m_crosshairUI = crosshairUI.GetComponent<InteractionUIPanel>();

            InitInteractionActions();

            m_inventoryHoverInfo = new InteractableItemCrosshairHoverInfo();

            if (useProximityItemHighlight)
            {
                InitProximityItemHighlight();
            }
        }

        private void InitProximityItemHighlight()
        {
            m_proximityItemHighlight = new ProximityItemHighlight(maxNumberHighlighedItems, proximityRangeRadius, m_player_cam, highlightableItemLayers);
        }

        private void InitInteractionActions()
        {
            InputActionMap = inputActionMapData.InputActionMap;

            // Use/interact button
            InputActionMap.PlayerInteractAction().started += InteractAction;
        }

        private void InteractAction(CallbackContext ctx)
        {
            if (!interactionableData.IsEmpty())
            {
                ItemInventoryInteractAction();
            }
        }

        private void ItemInventoryInteractAction()
        {
            InventoryItemBase _inventoryItem = interactionableData.GetInventoryItem();

            if (_inventoryItem != null)
            {
                AddItemToInventory(_inventoryItem);
            }
            else
            {
                interactionableData.Interact();

                UseEquipmentItem();
            }
        }

        private void UseEquipmentItem()
        {
            if (interactionableData.Interactable.IsWrongItemEqipped())
            {
                SetInventoryHoverInfo(inventoryData.WrongEquippedItemMsg);
            }
            else
            {
                // Currently removes all used equipment items
                RemoveCurrentlyUsedEquippedItem();

                CrosshairInteraction();
            }
        }

        private void AddItemToInventory(InventoryItemBase _inventoryItem)
        {
            if (inventoryData.OnAddToInventory(_inventoryItem))
            {
                interactionableData.Interact();
                CrosshairInteraction();

                if (_inventoryItem.shouldDestroyImmediately)
                {
                    UseItemImmediately(_inventoryItem);
                }
            }
            else
            {
                SetInventoryHoverInfo(inventoryData.InventoryFullMsg);
            }
        }

        private void SetInventoryHoverInfo(string _info)
        {
            m_inventoryHoverInfo.SetHoverInfo(interactionableData.Interactable.InteractableItemCrosshairHoverInfo);
            m_inventoryHoverInfo.CrosshairHoverText = _info;

            SetCrosshairHoverInfo(m_inventoryHoverInfo);
        }

        private void RemoveCurrentlyUsedEquippedItem()
        {
            if (interactionableData.Interactable.IsCorrectItemEqipped())
            {
                inventoryData.OnRemoveEqippedItemFromInventory(); 
            }
        }

        private void UseItemImmediately(InventoryItemBase _inventoryItem)
        {
            if (_inventoryItem.autoUse)
            {
                // Remove it right after adding it to the inventory (data)
                inventoryData.OnRemoveFromInventory(_inventoryItem);
            }

            interactionableData.Interactable.PlaySoundBeforeDestroy();
            ResetAppearance();
        }

        private void CrosshairInteraction()
        {
            // Hide crosshair when interacting (cliked on item) and show it again when not interacting
            // Needed if already hiting item with ray
            if (interactionableData.Interactable.IsCurrentlyInteracting)
            {
                SetCrosshairHoverInfo(InteractableItemCrosshairHoverInfo.emptyHoverInfo);
            }
            else
            {
                SetCrosshairHoverInfo(interactionableData.Interactable.InteractableItemCrosshairHoverInfo);
            }
        }

        private void SetCrosshairHoverInfo(InteractableItemCrosshairHoverInfo _crosshairHoverInfo)
        {
            m_crosshairUI.UpdateCrosshair(_crosshairHoverInfo);
        }

        void Update()
        {
            UpdateItemHoverDisplay(CheckForInteractableItem());

            if (useProximityItemHighlight)
            {
                m_proximityItemHighlight.CheckForHighlightableItemsInRange();
            }
        }

        private InteractableBase CheckForInteractableItem()
        {
            Ray _ray = new Ray(m_player_cam.transform.position, m_player_cam.transform.forward);
            RaycastHit _hitInfo;

            if(Physics.SphereCast(_ray, raySphereRadius, out _hitInfo, rayDistance, interactabletableLayers))
            {
                return _hitInfo.transform.GetComponent<InteractableBase>();
            }
            else
            {
                // Nothing is hit
                HideOrResetData();
            }

            return null;
        }

        private void UpdateItemHoverDisplay(InteractableBase _interactable)
        { 
            if(_interactable != null && _interactable.IsInteractable)
            {
                if(interactionableData.IsEmpty())
                {
                    interactionableData.Interactable = _interactable;

                    // Needed if item is interacted with but is not continously being hit with ray (ex: If it is moved in front of the crosshair after interacting with it, like with Quick Examine mode). 
                    if (!interactionableData.Interactable.IsCurrentlyInteracting)
                    {
                        // UI data
                        if (!useProximityItemHighlight)
                        {
                            interactionableData.HoverOver();
                        }

                        SetCrosshairHoverInfo(interactionableData.Interactable.InteractableItemCrosshairHoverInfo);
                    }
                }
                else
                {
                    // Check if we are hitting a new interactable, without clearing the previous one (e.g. two overlapping items)
                    // Also only switch current interactable if not interacting with an object already (prevents selecting another object via quick selection mode) 
                    if (!interactionableData.IsSameInteractable(_interactable) && !interactionableData.Interactable.IsCurrentlyInteracting)
                    {
                        if (!useProximityItemHighlight)
                        {
                            interactionableData.ResetAppearance();  // Clear previously highlighted item
                        }

                        interactionableData.Interactable = _interactable;

                        if (!useProximityItemHighlight)
                        {
                            interactionableData.HoverOver();
                        }

                        SetCrosshairHoverInfo(interactionableData.Interactable.InteractableItemCrosshairHoverInfo);
                    }
                }
            }
            else
            {
                // If we hit something, but there is no data to display
                HideOrResetData();
            }
        }

        private void HideOrResetData()
        {
            if (!interactionableData.IsEmpty())
            {
                // Hide crosshair completely if interacting with an object, otherwise show the default crosshair
                if (interactionableData.Interactable.IsCurrentlyInteracting)
                {
                    SetCrosshairHoverInfo(InteractableItemCrosshairHoverInfo.emptyHoverInfo);
                }
                else
                {
                    // Reset data/appearance when no longer interacting with an item
                    ResetAppearance();
                    ResetData();
                }
            }
        }

        private void ResetData()
        {
            interactionableData.ResetData();
        }

        private void ResetAppearance()
        {
            SetCrosshairHoverInfo(InteractableItemCrosshairHoverInfo.defaultHoverInfo);

            // Proximity highlight manages the appearance directly
            if (!useProximityItemHighlight)
            {
                interactionableData.ResetAppearance();
            }
        }

        // Must manually enable/disable actions
        private void OnEnable()
        {
            InputActionMap.Enable();
        }

        private void OnDisable()
        {
            InputActionMap.Disable();

            if (m_proximityItemHighlight != null)
            {
                m_proximityItemHighlight.ClearCache();
            }
        }
    }
}
