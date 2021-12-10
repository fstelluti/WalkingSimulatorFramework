using NaughtyAttributes;
using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace WalkingSimFramework.Interactable_System
{
    /// <summary>
    /// Base class for examinable items, which will toggle the standard inputs to be able to rotate items in front of the player.
    /// </summary>
    public abstract class ExaminableItemBase : InteractableBase
    {
        [Space, Header("Examineable Settings")]
        [Tooltip("Distance scale the item should be from the camera. Smaller values are closer.")]
        [Range(0.2f, 1.0f)] [SerializeField] protected float distanceScaleToCamera = 0.5f;
        [Tooltip("How fast to rotate the item when examining it")]
        [MinValue(0f)] [SerializeField] private float rotationSpeed = 120f;

        [Tooltip("Enable restricting the rotation about the horizontal (relative to the screen) axis")]
        [SerializeField] protected bool enableRotationXLimit;
        [Tooltip("Angle limit rotation on the horizontal (relative to the screen) axis")]
        [EnableIf("enableRotationXLimit")]
        [MinValue(0f), MaxValue(360f)] [SerializeField] private float rotationXLimit = 25.0f;
        [Tooltip("Enable to orient the item so the top faces the camera")]
        [SerializeField] protected bool useReadingOrientation;

        protected Camera m_player_cam;

        // Initial item transform information
        protected Vector3 m_originalPosition;
        protected Quaternion m_originalRotation;
        protected Vector3 m_originalUp;

        // Input values
        private Vector2 m_rotateValue;

        protected float m_totalAngle;
        protected Vector3 m_initXAxis;

        // Used to center the item in front of the camera when examining it, instead of using the pivot point.
        protected Vector3 m_centerDir;
        private Renderer m_itemRenderer;

        protected override void Awake()
        {
            base.Awake();

            m_player_cam = Camera.main;  // Get first camera tagged with 'MainCamera'

            m_originalPosition = transform.position;
            m_originalRotation = transform.rotation;
            m_originalUp = transform.up;

            m_itemRenderer = GetComponent<Renderer>();

            Vector3 _centerPosition = m_itemRenderer.bounds.center;
            m_centerDir = transform.position - _centerPosition;
        }
        public override void OnInteract()
        {
            base.OnInteract();
        }

        protected void OnUseOrPickUpItem()
        {
            bool _equipmentUsed = OnUseEquippedItem();

            // Restrict to one event at a time for now
            if (!_equipmentUsed)
            {
                itemSounds.TogglePickupPutBackSound();

                OnItemInteractEvent();
            }
        }

        protected Vector3 GetCameraTargetPosition()
        {
            return (m_player_cam.transform.position + m_player_cam.transform.forward * distanceScaleToCamera) + m_centerDir;
        }

        /// <summary>
        /// Toggles between regular inputs and moving the object manually. 
        /// Can also zoom in on the object as that input is not affected. 
        /// </summary>
        /// <param name="_isEnabled"> Enable examination of the item or return to normal input</param>
        public void ToggleInputs(bool _isEnabled)
        {
            // TODO: Had issues in the past where this was true. May no longer be the case.
            if (InpActionMap == null)
            {
                Debug.LogError("Error: InputActionMap should not be null");
                return;
            }

            if(_isEnabled)
            {
                InpActionMap.GetBasicInputs().ForEach(action => action.Disable());

                InpActionMap.PlayerMoveExamineAction().performed += SetRotateVal;
                InpActionMap.PlayerMoveExamineAction().canceled += _ => m_rotateValue = Vector2.zero;
            }
            else
            {
                InpActionMap.GetBasicInputs().ForEach(action => action.Enable());

                InpActionMap.PlayerMoveExamineAction().performed -= SetRotateVal;
            }

        }

        private void SetRotateVal(CallbackContext ctx)
        {
            m_rotateValue = ctx.ReadValue<Vector2>();
        }

        protected void RotateExaminedItem()
        {
            // Can rotate in both directions at the same time
            if (Math.Abs(m_rotateValue.x) > 0.5)
            {
                Vector3 _XRotationAxis = useReadingOrientation ? transform.forward : transform.up;

                transform.RotateAround(m_itemRenderer.bounds.center, _XRotationAxis * m_rotateValue.x, Time.deltaTime * rotationSpeed);
            }
            
            if (Math.Abs(m_rotateValue.y) > 0.5)
            {
                // This will add +/- 1 at each frame, giving an estimation of the angle 
                m_totalAngle += m_rotateValue.y;

                // Adjust X angle limit, as it should more closely resemble the actual rotation limit
                float _adjustedXLimit = rotationXLimit * 3;

                // Only rotate if in range or if we always allow it
                if (!enableRotationXLimit || m_totalAngle >= -_adjustedXLimit * 2 && m_totalAngle <= _adjustedXLimit * 2)
                {
                    transform.RotateAround(m_itemRenderer.bounds.center, m_initXAxis * m_rotateValue.y, -1.0f * Time.deltaTime * rotationSpeed);
                }
                else
                {
                    // Cancel out the increment/decrement if out of range
                    m_totalAngle -= m_rotateValue.y;
                }
            }
        }
    }
}
