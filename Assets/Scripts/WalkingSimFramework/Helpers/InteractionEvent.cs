using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace WalkingSimFramework.Helpers
{
    /// <summary>
    /// Class that encapsulates a Unity Event to have better control on how it is called
    /// </summary>
    [System.Serializable]
    public class InteractionEvent
    {
        /// <summary>
        /// Used to describe event frequencies 
        /// </summary>
        public enum InteractionEventFrequency
        {
            ONCE = 0,
            TOGGLE = 1,
            ALWAYS = 2
        }


        [SerializeField] private InteractionEventFrequency iteractionEventFrequency;

        [SerializeField] private EventContainer ActivationEvent;
        [ShowIf("IsToggleSelected")]
        [AllowNesting]
        [SerializeField] private EventContainer DeActivationEvent;

        [ShowIf("IsToggleSelected")]
        [AllowNesting]
        [SerializeField] private bool StartActivated = false;

        bool m_activateOnce = true;

        public bool IsToggleSelected()
        {
            return iteractionEventFrequency == InteractionEventFrequency.TOGGLE;
        }

        public void InvokeInteraction()
        {
            switch(iteractionEventFrequency)
            {
                case InteractionEventFrequency.ONCE:
                    ActivateEventOnce();
                    break;
                case InteractionEventFrequency.TOGGLE:
                    ToggleEvent();
                    break;
                case InteractionEventFrequency.ALWAYS:
                    AlwaysActivateEvent();
                    break;
                default:
                    AlwaysActivateEvent();
                    break;
            }
        }

        private void ActivateEventOnce()
        {
            if(m_activateOnce)
            {
                ActivationEvent.Event.Invoke();
                m_activateOnce = false;
            }
        }

        private void ToggleEvent()
        {
            if (StartActivated)
            {
                DeActivationEvent.Event.Invoke();
            }
            else
            {
                ActivationEvent.Event.Invoke();
            }

            StartActivated = !StartActivated;
        }

        private void AlwaysActivateEvent()
        {
            ActivationEvent.Event.Invoke();
        }

        // Used only to be able to hide from the inspector
        [System.Serializable]
        private class EventContainer
        {
            public UnityEvent Event;
        }
    }

}
