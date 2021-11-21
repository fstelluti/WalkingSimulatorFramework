using WalkingSimFramework.Scriptable_Objects.Inventory;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WalkingSimFramework.UI_System.InventoryUI
{
    /// <summary>
    /// Demo class to store the item in the UI
    /// </summary>
    public class ItemInventorySlot : MonoBehaviour
    {
        public event Action<ItemInventorySlot> OnInventoryButtonClickEvent; // Delegate on click button event

        [SerializeField] GameObject itemCount;
        [SerializeField] GameObject itemDesc;
        [SerializeField] GameObject itemImage;

        [SerializeField] Color selectionSlotColor = Color.white;

        InventoryItemBase m_item;
        int m_itemAmount;

        TextMeshProUGUI m_itemCountText;
        TextMeshProUGUI m_itemDescriptionText;

        Image m_itemImage;

        Image m_slotImage;
        Color m_originalColor;

        public int ItemAmount { get => m_itemAmount; }

        public InventoryItemBase Item
        {
            get { return m_item;  }
            set
            {
                m_item = value;

                bool IsItemValid = m_item != null;

                if (IsItemValid)
                {
                    m_itemDescriptionText.SetText(m_item.shortDescription);
                    m_itemImage.sprite = m_item.itemSprite;
                }

                EnableSlotComponents(IsItemValid);
            }
        }

        private void Awake()
        {
            m_itemCountText = itemCount.GetComponent<TextMeshProUGUI>();
            m_itemDescriptionText = itemDesc.GetComponent<TextMeshProUGUI>();
            m_itemImage = itemImage.GetComponent<Image>();

            m_slotImage = GetComponent<Image>();
            m_originalColor = m_slotImage.color;

            m_itemAmount = 0;
        }

        private void OnEnable()
        {
            // Hide if we don't have an item associated with it
            EnableSlotComponents(m_item != null);
        }

        public void OnClickSlot()
        {
            if(m_item != null && OnInventoryButtonClickEvent != null)
            {
                OnInventoryButtonClickEvent(this);
            }
        }

        private void EnableSlotComponents(bool _enable)
        {
            m_itemImage.enabled = _enable;
            m_itemCountText.enabled = _enable;
            m_itemDescriptionText.enabled = _enable;
        }

        public void IncreaseItemSlotCount()
        {
            m_itemAmount++;
            m_itemCountText.SetText(m_itemAmount.ToString());
        }

        public void DecreaseItemSlotCount()
        {
            if(m_itemAmount > 0)
            {
                m_itemAmount--;
                m_itemCountText.SetText(m_itemAmount.ToString());
            }
        }

        public void ResetItemSlotCount()
        {
            m_itemAmount = 0;
        }

        public bool HasRoomInSlot()
        {
            return ItemAmount < Item.maxStacks;
        }

        public void SetAmountCountActive(bool _isActive)
        {
            itemCount.SetActive(_isActive);
        }

        public void SelectItemSlot()
        {
            m_slotImage.color = selectionSlotColor;
        }

        public void ClearSelection()
        {
            m_slotImage.color = m_originalColor;
        }

        public bool IsSelected()
        {
            return m_slotImage.color == selectionSlotColor;
        }
    }
}
