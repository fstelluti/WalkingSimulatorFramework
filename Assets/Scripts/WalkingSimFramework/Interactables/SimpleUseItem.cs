using WalkingSimFramework.Interactable_System;

namespace WalkingSimFramework.Interactables
{
    /// <summary>
    /// Use for game objects that have simple interactions (e.g. switches/buttons, consumable items, static items like pianos] etc).
    /// </summary>
    public class SimpleUseItem : InteractableBase
    {
        protected override void Awake()
        {
            base.Awake();
        }

        public override void OnInteract()
        {
            base.OnInteract();

            bool _equipmentUsed = OnUseEquippedItem();

            // Restrict to one event at a time for now
            if(!_equipmentUsed)
            {
                itemSounds.TogglePickupPutBackSound();

                OnItemInteractEvent();
            }
        }
    }
}
