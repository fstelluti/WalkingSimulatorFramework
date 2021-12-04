using WalkingSimFramework.Helpers;
using WalkingSimFramework.Scriptable_Objects;
using WalkingSimFramework.Scriptable_Objects.Events;
using WalkingSimFramework.Scriptable_Objects.Inventory;
using SmartData.SmartEquippedPanelData;
using SmartData.SmartInt;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

namespace WalkingSimFramework.UI_System.InventoryUI
{
    /// <summary>
    /// Demo class to manage the Inventory UI.
    /// </summary>
    public class IntentoryContainerUI : MonoBehaviour
    {
        [Space, Header("Interaction Data")]
        [SerializeField] private InputActionMapData inputActionMapData = null;
        [SerializeField] private InventoryData inventoryData = null;

        [Space, Header("Smart Data")]
        [SerializeField] IntWriter healthVar;
        [SerializeField] EquippedPanelDataWriter equippedItemVar;

        [Space]
        [SerializeField] Transform notePopupPanel;

        [Space]
        [Tooltip("Button that should be selected when opening the inventory")]
        [SerializeField] Button firstSelectedButton;

        [Space, Header("Item types")]
        [SerializeField] Transform consumableItemParent;
        [SerializeField] Transform equipItemParent;
        [SerializeField] Transform notesParent;

        ItemInventorySlot[] m_ConsumableItemSlots;
        ItemInventorySlot[] m_EquipItemSlots;
        ItemInventorySlot[] m_NoteInventorySlots;

        TextMeshProUGUI m_noteText;
        Button m_notePopupButton;

        ItemInventorySlot m_currentlySelectedEquipmentItemSlot;

        EquippedPanelData m_EquippedData;

        InputActionWrapper m_InputActionMap;

        void Awake()
        {
            m_InputActionMap = inputActionMapData.InputActionMap;

            gameObject.SetActive(false);  // Hide initially

            InitInventoryPanelActions();

            inventoryData.addToInventoryDelegate += AddToInventory;
            inventoryData.removeEqipmentFromInventoryDelegate += RemoveEquippedItem;

            m_noteText = notePopupPanel.GetComponentInChildren<TextMeshProUGUI>();  // Assume first child is the correct one to display full note text

            m_ConsumableItemSlots = consumableItemParent.GetComponentsInChildren<ItemInventorySlot>();
            m_NoteInventorySlots = notesParent.GetComponentsInChildren<ItemInventorySlot>();
            m_EquipItemSlots = equipItemParent.GetComponentsInChildren<ItemInventorySlot>();

            m_notePopupButton = notePopupPanel.gameObject.GetComponentInChildren<Button>();

            SetupConsumeItemEvents();
            SetupEquipItemEvents();
            SetupNotesItemEvents();

            m_EquippedData = new EquippedPanelData();
        }

        private void SetupNotesItemEvents()
        {
            foreach (ItemInventorySlot noteSlot in m_NoteInventorySlots)
            {
                noteSlot.OnInventoryButtonClickEvent += NoteDisplayFromSlot;
            }
        }

        private void SetupEquipItemEvents()
        {
            foreach (ItemInventorySlot equipSlot in m_EquipItemSlots)
            {
                equipSlot.OnInventoryButtonClickEvent += EquipItemFromSlot;
            }
        }

        private void SetupConsumeItemEvents()
        {
            foreach (ItemInventorySlot consumeSlot in m_ConsumableItemSlots)
            {
                consumeSlot.OnInventoryButtonClickEvent += ConsumeItemFromSlot;
            }
        }

        private void ConsumeItemFromSlot(ItemInventorySlot _itemSlot)
        {
            ConsumeItem(_itemSlot.Item);

            if(_itemSlot.ItemAmount == 1)
            {
                inventoryData.OnRemoveFromInventory(_itemSlot.Item);
            }
    
            RemoveFromConsumableItemsInventory(_itemSlot);
        }

        private void ConsumeItem(InventoryItemBase _item)
        {
            ConsumableInventoryItem _consumeItem = (ConsumableInventoryItem) _item;
            healthVar.value = _consumeItem.itemValue;
        }

        private void EquipItemFromSlot(ItemInventorySlot _itemSlot)
        {
            EquipableInventoryItem _equipItem = ((EquipableInventoryItem)_itemSlot.Item);

            if (_itemSlot.IsSelected())
            {
                // Deselect item
                m_EquippedData = EquippedPanelData.emptyEqipmentData;
            }
            else
            {
                m_EquippedData.Description = _equipItem.shortDescription;
                m_EquippedData.ItemSprite = _equipItem.itemSprite;
                m_EquippedData.ItemType = _equipItem.equipItemType;

                m_currentlySelectedEquipmentItemSlot = _itemSlot;
            }

            equippedItemVar.value = m_EquippedData;

            // Could also use a Toggle group here
            ToggleEquipItemSelection(_itemSlot);
        }

        private void ToggleEquipItemSelection(ItemInventorySlot _itemSlot)
        {
            foreach (ItemInventorySlot _slot in m_EquipItemSlots)
            {
                if (_slot == _itemSlot)
                {
                    if (_itemSlot.IsSelected())
                    {
                        _slot.ClearSelection();
                    }
                    else
                    {
                        _itemSlot.SelectItemSlot();
                    }
                }
                else
                {
                    _slot.ClearSelection();
                }
            }
        }

        private void NoteDisplayFromSlot(ItemInventorySlot _itemSlot)
        {
            // Display popup with full note text
            NoteInventoryItem _noteItem = ((NoteInventoryItem) _itemSlot.Item);

            m_noteText.text = _noteItem.noteDescription;
            notePopupPanel.gameObject.SetActive(true);

            EventSystem.current.SetSelectedGameObject(m_notePopupButton.gameObject);
        }

        private void InitInventoryPanelActions()
        {
            m_InputActionMap.UIOpenAction().started += ToggleMenuAction;
        }

        private void ToggleMenuAction(CallbackContext ctx)
        {
            bool _isActive = gameObject.activeSelf;
            bool _isRegularInputEnabled = m_InputActionMap.PlayerMoveAction().enabled;

            // Hackish, but want to prevent opening up Inventory when examining an item (i.e. when can't move normally)
            if(_isRegularInputEnabled || _isActive)
            {
                ToggleInputs(!_isActive);
                gameObject.SetActive(!_isActive);

                if(!_isActive)
                {
                    SelectFirstButton();
                }
            }
        }

        public bool AddToInventory(InventoryItemBase _item)
        {
            bool _IsUpdated = false;

            if (_item != null)
            {
                if (_item.GetItemType() == InventoryItemType.CONSUMABLE)
                {
                    if(_item.autoUse)
                    {
                        ConsumeItem(_item);
                        _IsUpdated = true;
                    }
                    else
                    {
                        _IsUpdated = AddToConsumableItemsInventory(_item);
                    }
                }
                else if(_item.GetItemType() == InventoryItemType.NOTE)
                {
                    _IsUpdated = AddToNoteItemsInventory(_item);
                }
                else if(_item.GetItemType() == InventoryItemType.EQUIPABLE)
                {
                    _IsUpdated = AddToEquipableItemsInventory(_item);
                }
            }

            return _IsUpdated;
        }

        public void RemoveEquippedItem()
        {
            if (equippedItemVar.isValid)
            {
                equippedItemVar.value = EquippedPanelData.emptyEqipmentData;

                // Remove from inventory data and UI
                if(m_currentlySelectedEquipmentItemSlot != null)
                {
                    inventoryData.OnRemoveFromInventory(m_currentlySelectedEquipmentItemSlot.Item);

                    m_currentlySelectedEquipmentItemSlot.ClearSelection();
                    RemoveFromEquipItemsInventory(m_currentlySelectedEquipmentItemSlot);

                    m_currentlySelectedEquipmentItemSlot = null;
                }
            }
        }

        private bool AddToConsumableItemsInventory(InventoryItemBase _item)
        {
            for(int i=0; i< m_ConsumableItemSlots.Length; i++)
            {
                ItemInventorySlot _itemInventorySlot = m_ConsumableItemSlots[i];

                if (_itemInventorySlot.Item == null)
                {
                    _itemInventorySlot.Item = Instantiate(_item);  // Make unique ref 
                    _itemInventorySlot.IncreaseItemSlotCount();
                    return true;
                }

                if (_itemInventorySlot.Item.isStackable && _itemInventorySlot.HasRoomInSlot())
                {
                    _itemInventorySlot.IncreaseItemSlotCount();
                    return true;
                }
            }

            return false;
        }

        private bool AddToEquipableItemsInventory(InventoryItemBase _item)
        {
            for (int i = 0; i < m_EquipItemSlots.Length; i++)
            {
                ItemInventorySlot _itemInventorySlot = m_EquipItemSlots[i];

                if (_itemInventorySlot.Item == null)
                {
                    _itemInventorySlot.Item = Instantiate(_item);  // Make unique ref 
                    _itemInventorySlot.IncreaseItemSlotCount();
                    return true;
                }

                if (_itemInventorySlot.Item.isStackable && _itemInventorySlot.HasRoomInSlot())
                {
                    _itemInventorySlot.IncreaseItemSlotCount();
                    return true;
                }
            }

            return false;
        }

        private void RemoveFromConsumableItemsInventory(ItemInventorySlot _itemSlot)
        {
            for (int i = 0; i < m_ConsumableItemSlots.Length; i++)
            {
                if(m_ConsumableItemSlots[i] == _itemSlot)
                {
                    if(_itemSlot.Item.isStackable && _itemSlot.ItemAmount > 1)
                    {
                        _itemSlot.DecreaseItemSlotCount();
                        return;
                    }

                    _itemSlot.Item = null;
                    _itemSlot.ResetItemSlotCount();
                    return;
                }
            }
        }

        private void RemoveFromEquipItemsInventory(ItemInventorySlot _itemSlot)
        {
            for (int i = 0; i < m_EquipItemSlots.Length; i++)
            {
                if (m_EquipItemSlots[i] == _itemSlot)
                {
                    // Assume equipment items are not stackable in this UI
                    _itemSlot.Item = null;
                    _itemSlot.ResetItemSlotCount();
                    return;
                }
            }
        }

        private bool AddToNoteItemsInventory(InventoryItemBase _item)
        {
            for (int i = 0; i < m_NoteInventorySlots.Length; i++)
            {
                ItemInventorySlot _itemInventorySlot = m_NoteInventorySlots[i];

                if (_itemInventorySlot.Item == null)
                {
                    _itemInventorySlot.Item = Instantiate(_item);  // Make unique ref 
                    _itemInventorySlot.SetAmountCountActive(false);
                    return true;
                }
                else 
                {
                    // Empty or null IDs will be considered 'full' for now.
                    if(!string.IsNullOrEmpty(_item.id))
                    {
                        if(_itemInventorySlot.Item.id.Equals(_item.id))
                        {
                            // Already added
                            return true;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }

            return false;
        }

        private void SelectFirstButton()
        {
            if(m_ConsumableItemSlots.Length > 0)
            {
                // Need to clear selection first
                EventSystem.current.SetSelectedGameObject(null);

                firstSelectedButton.Select();
            }
        }

        // Similar to what is done in ExaminableItemBase
        private void ToggleInputs(bool _isEnabled)
        {
            // Might still have issues with this being null
            if (m_InputActionMap == null)
            {
                Debug.LogError("Error: InputActionMap should not be null");
                return;
            }

            if (_isEnabled)
            {
                m_InputActionMap.GetBasicInputs().ForEach(action => action.Disable());

                m_InputActionMap.PlayerInteractAction().Disable();
            }
            else
            {
                m_InputActionMap.GetBasicInputs().ForEach(action => action.Enable());

                m_InputActionMap.PlayerInteractAction().Enable();
            }
        }

        private void OnDestroy()
        {
            inventoryData.addToInventoryDelegate -= AddToInventory;
            inventoryData.removeEqipmentFromInventoryDelegate -= RemoveEquippedItem;
        }
    }
}
