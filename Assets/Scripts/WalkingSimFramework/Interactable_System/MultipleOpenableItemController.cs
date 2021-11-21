using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace WalkingSimFramework.Interactable_System
{
    /// <summary>
    /// Used for opening two (or more) openable items at the same time (ex: double doors)
    /// Assume either all items are locked or unlocked. If only some are locked then they probably shouldn't be linked. 
    /// </summary>
    public class MultipleOpenableItemController : MonoBehaviour
    {
        [InfoBox("List of openable items that should be linked together so that they can be opened/closed at the same time.")]
        [Space, Header("Multiple Openable Item Settings")]
        [SerializeField] private List<OpenableBase> linkedOpenableItems;

        private void OpenAllItems(bool _isLocked, string _crosshairText)
        {
            // Synchronize door status
            foreach (OpenableBase openItem in linkedOpenableItems)
            {
                openItem.IsLocked = _isLocked;
                openItem.InteractableItemCrosshairHoverInfo.CrosshairHoverText = _crosshairText;

                openItem.OpenItemAnimation();
            }
        }

        private void OnEnable()
        {
            foreach (OpenableBase openItem in linkedOpenableItems)
            {
                openItem.onOpenInteractAction += OpenAllItems;
            }
        }

        private void OnDisable()
        {
            foreach (OpenableBase openItem in linkedOpenableItems)
            {
                openItem.onOpenInteractAction -= OpenAllItems;
            }
        }

    }
}
