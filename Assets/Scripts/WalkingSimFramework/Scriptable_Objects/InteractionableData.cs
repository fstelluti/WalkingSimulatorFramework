using WalkingSimFramework.Interactable_System;
using WalkingSimFramework.Scriptable_Objects.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WalkingSimFramework.Scriptable_Objects
{
    /// <summary>
    /// Data class that is essentially a container for managing an InteractableBase object.
    /// </summary>
    [CreateAssetMenu(fileName = "Interactable Data", menuName = "WalkingSimFramework/InteractionData")]
    public class InteractionableData : ScriptableObject
    {
        private InteractableBase m_interactableBase;

        // Prop to hold the Interactable script (used on a gameobject)
        public InteractableBase Interactable
        {
            get => m_interactableBase;
            set => m_interactableBase = value;
        }

        public void HoverOver()
        {
            m_interactableBase.OnHoverOver();
        }

        public InventoryItemBase GetInventoryItem()
        {
            return m_interactableBase.GetInventorytItem();
        }

        public void Interact()
        {
            if(m_interactableBase.IsInteractable)
            {
                m_interactableBase.OnInteract();
            }
        }

        public void ResetAppearance()
        {
            m_interactableBase.ResetAppearance();
        }

        // Status methods

        public bool IsEmpty() => m_interactableBase == null;

        public bool IsSameInteractable(InteractableBase _newInteractable) => m_interactableBase == _newInteractable;

        public void ResetData()
        {
            if(m_interactableBase != null)
            {
                m_interactableBase = null;
            }
        }

    }
}
