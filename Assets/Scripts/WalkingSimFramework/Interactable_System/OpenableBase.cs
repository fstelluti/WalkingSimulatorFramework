using NaughtyAttributes;
using UnityEngine;

namespace WalkingSimFramework.Interactable_System
{
    /// <summary>
    /// Base class for any item type that can be opened (Door, drawer, etc).
    /// </summary>
    public abstract class OpenableBase : InteractableBase
    {
        [Space, Header("Openable Settings")]
        [Tooltip("Should start locked")]
        [SerializeField] private bool isLocked;
        [Tooltip("Text when item is locked")]
        [SerializeField] private string lockedText;
        [Tooltip("Text when item can be opened")]
        [SerializeField] private string openText;
        [Tooltip("Text when item can be closed")]
        [SerializeField] private string closedText;

        [InfoBox("Link to multiple items so that they can be opened/closed simultaneously. " +
            "Needs to be used in conjuntion with another controller (MultipleOpenableItemController), as items should not be aware of which items they are linked to." +
            " If this gameobject is linked without a controller, then it will not open.")]
        [Space, Header("Link Item Settings")]
        [SerializeField] private bool isLinked;

        [Space, Header("Animation Settings")]
        [Tooltip("Curve to control the opening/closing animation")]
        [SerializeField] protected AnimationCurve openingCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        [SerializeField] protected float openingTransitionDuration = 1f;

        // If next action is to 'close' i.e. back to initial state
        protected bool m_shouldCloseNext = true;

        // Used to link multiple openable items (ex: double doors) together
        public delegate void OpenableInteractAction(bool _isLocked, string _crosshairText);
        public event OpenableInteractAction onOpenInteractAction;

        protected bool m_pauseCoroutine;
        bool m_hasColliderWithoutTrigger;

        public bool IsLocked { get => isLocked; set => isLocked = value; }

        protected override void Awake()
        {
            base.Awake();

            Collider[] _itemColliders = GetComponents<Collider>();

            foreach(Collider collider in _itemColliders)
            {
                if(collider != null && !collider.isTrigger)
                {
                    m_hasColliderWithoutTrigger = true;
                    break;
                }
            }
        }

        protected void OpenItemAction(bool _isLocked, string _crosshairText)
        {
            if(onOpenInteractAction != null)
            {
                onOpenInteractAction(_isLocked, _crosshairText);
            }
        }

        public virtual void OpenItemAnimation()
        {
            // If animation is paused, then resume it when interacting with item again
            if (m_pauseCoroutine)
            {
                m_pauseCoroutine = false;
            }
        }

        protected virtual void OnTriggerEnter(Collider _other)
        {
            // Only pause coroutines if can have a collision
            if (m_shouldCloseNext && m_hasColliderWithoutTrigger)
            {
                m_pauseCoroutine = true;
            }
        }

        public override void OnInteract()
        {
            base.OnInteract();

            OnOpenItem();

            // If linked, run an action that is managed elsewhere
            if (isLinked)
            {
                OpenItemAction(IsLocked, InteractableItemCrosshairHoverInfo.CrosshairHoverText);
            }
            else
            {
                OpenItemAnimation();
            }
        }

        private void OnOpenItem()
        {
            if (IsLocked)
            {
                Unlock();
            }
            else if (m_shouldCloseNext)
            {
                itemSounds.ToggleOpenCloseSound();
                SetCrosshairHoverText(closedText);
            }
            else
            {
                // Closing
                itemSounds.ToggleOpenCloseSound();
                SetCrosshairHoverText(openText);
            }
        }

        private void Unlock()
        {
            if (OnUseEquippedItem())
            {
                IsLocked = false;
                itemSounds.ToggleOpenCloseSound();
                SetCrosshairHoverText(closedText);
            }
            else
            {
                itemSounds.PlayBlockedSound();
                OnItemInteractEvent();  // Can't call both events for now
                SetCrosshairHoverText(lockedText);
            }
        }

        private void SetCrosshairHoverText(string _hoverText)
        {
            if (!string.IsNullOrEmpty(_hoverText))
            {
                InteractableItemCrosshairHoverInfo.CrosshairHoverText = _hoverText;
            }
        }
    }
}
