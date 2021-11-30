using WalkingSimFramework.Interactable_System;
using NaughtyAttributes;
using System.Collections;
using UnityEngine;

namespace WalkingSimFramework.Interactables
{
    /// <summary>
    /// Used for examining items with animations to move it in front of the player and back to the original position.
    /// </summary>
    public class AnimatedExaminableItem : ExaminableItemBase
    {
        [Space, Header("Rotation/Translation Animation Settings")]
        [Tooltip("How fast the item should rotate when moving to the camera")]
        [MinValue(5.0f)] [SerializeField] private float roatateToAnimSpeed = 25.0f;
        [Tooltip("How fast the item should rotate when moving away from the camera")]
        [MinValue(5.0f)] [SerializeField] private float roatateBackAnimSpeed = 35.0f;
        [Tooltip("How fast item should move to/from position in from of the camera")]
        [MinValue(1.0f)] [SerializeField] private float movementAnimSpeed = 3.0f;

        // Animation 
        IEnumerator m_RotateItemRoutine;
        IEnumerator m_TranslateItemRoutine;

        WaitForSeconds m_waitForCoroutine;

        // Speed multipliers, to avoid large inspector values
        float m_speedMultiplierTo = 10f;
        float m_speedMultiplierBack = 25f;
        float m_maxRelativeSpeedMultiplier = 5f;

        float m_waitTimeForCoroutine = 0.2f;

        bool m_isExaminingTranslateItem;
        bool m_isExaminingRotateItem;

        bool m_isRotateCoroutineRunning;
        bool m_isTranslateCoroutineRunning;

        protected override void Awake()
        {
            base.Awake();

            m_waitForCoroutine = new WaitForSeconds(m_waitTimeForCoroutine);
        }

        public override void OnInteract()
        {
            base.OnInteract();

            bool _areCoroutinesRunning = m_isTranslateCoroutineRunning || m_isRotateCoroutineRunning;

            // Can only interact with the item when animations are not running
            if (!_areCoroutinesRunning)
            {
                IsCurrentlyInteracting = !IsCurrentlyInteracting;

                OnUseOrPickUpItem();

                // Enable/Disable needed input actions
                ToggleInputs(IsCurrentlyInteracting);

                // Re-apply any effects - will likely be applied before the animations are finished, but easier to do this here. 
                OnHoverOver();

                if (IsCurrentlyInteracting)
                {
                    ResetAppearance();
                    m_totalAngle = 0f;
                }
            }
        }

        private void Update()
        {
            bool _areCoroutinesRunning = m_isTranslateCoroutineRunning || m_isRotateCoroutineRunning;

            // Block from rotating item unless examining it and animations are not running
            if (IsCurrentlyInteracting && !_areCoroutinesRunning)
            {
                RotateExaminedItem();
            }

            // Prevent interrupting an animation
            if (!_areCoroutinesRunning)
            {
                RotateAnim();
                MoveAnim();
            }
        }

        /// <summary>
        /// Coroutine used to animate the rotation of the object. Will rotate to face to camera when interacting and rotate back when not.
        /// Note that if the item is rotated more than 80 degrees, then put back, the rotation speed will increase by roatateFlippedAnimSpeed.
        /// </summary>
        private IEnumerator ChangeItemRotationRoutine()
        {
            m_isRotateCoroutineRunning = true;

            if (!m_isExaminingRotateItem)
            {
                // Wait a bit to give time for the FPS controller to stop, so that the item can be centered properly in front of the camera. 
                yield return m_waitForCoroutine;
            }

            // Use either original 'up' direction, or the forward direction of the camera (but flipped since the object is facing the camera)
            Vector3 _targetDirection = m_isExaminingRotateItem ? (m_originalUp - m_originalPosition).normalized : (m_player_cam.transform.forward * -1.0f);

            float _rotationCorrection_X = 0f;

            // Initially there is no rotation applied, else we rotate it 90 degrees so that the top faces the camera and if that orientation is needed.
            if (!m_isExaminingRotateItem && useReadingOrientation)
            {
                _rotationCorrection_X = 90.0f;
            }

            Quaternion _targetRotation = m_isExaminingRotateItem ? m_originalRotation : Quaternion.LookRotation(_targetDirection);
            _targetRotation *= Quaternion.Euler(_rotationCorrection_X, 0f, 0f);

            Vector3 _targetPosition = m_isExaminingRotateItem ? m_originalPosition : GetCameraTargetPosition();

            float _relativeSpeed = m_isExaminingRotateItem ? roatateBackAnimSpeed * m_speedMultiplierBack : roatateToAnimSpeed * m_speedMultiplierTo;
            _relativeSpeed *= Mathf.Clamp(movementAnimSpeed * (_targetPosition - transform.position).magnitude, 1.0f, movementAnimSpeed + m_maxRelativeSpeedMultiplier); // If distance is large, speed is large;

            // Use a high tolorance, instead of close to zero, to make sure this coroutine finishes since RotateTowards will not overshoot. 
            while (Quaternion.Angle(transform.rotation, _targetRotation) > 2.0f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, Time.deltaTime * _relativeSpeed);
                yield return null;
            }

            m_isExaminingRotateItem = !m_isExaminingRotateItem;

            // Save the rotation axis so that it is always the same when rotating the object manually.
            // Need this here because the axis needs to be relative to the final rotation.
            m_initXAxis = transform.right;

            // Make sure that the rotation is exactly at the target
            if (Quaternion.Angle(transform.rotation, _targetRotation) < 2.0f)
            {
                transform.rotation = _targetRotation;
            }

            m_isRotateCoroutineRunning = false;
        }

        /// <summary>
        /// Coroutine used to animate the translation of the object. Will move the object in front of the camera at a certain distance.
        /// </summary>
        private IEnumerator ChangeItemTranslationRoutine()
        {
            m_isTranslateCoroutineRunning = true;

            if (!m_isExaminingTranslateItem)
            {
                // Wait a bit to give time for the FPS controller to stop, so that the item can be centered properly in front of the camera. 
                yield return m_waitForCoroutine;
            }

            // Logic to toggle positions
            // The target positionis offset to make sure that the item is centered in front of the camera
            Vector3 _targetPosition = m_isExaminingTranslateItem ? m_originalPosition : GetCameraTargetPosition();
            float _relativeSpeed = Mathf.Clamp(movementAnimSpeed * (_targetPosition - transform.position).magnitude, 1.0f, movementAnimSpeed + m_maxRelativeSpeedMultiplier); // If distance is large, speed is large

            while (transform.position != _targetPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * _relativeSpeed);
                yield return null;
            }

            m_isExaminingTranslateItem = !m_isExaminingTranslateItem;

            m_isTranslateCoroutineRunning = false;
        }

        // Handles when to start the rotation coroutine.
        // Should only run one at a time.
        private void RotateAnim()
        {
            if (IsCurrentlyInteracting)
            {
                if (!m_isExaminingRotateItem)
                {
                    StartRotateCoroutine();
                }
            }
            else
            {
                if (m_isExaminingRotateItem)
                {
                    StartRotateCoroutine();
                }
            }
        }

        private void StartRotateCoroutine()
        {
            if (m_RotateItemRoutine != null)
            {
                StopCoroutine(m_RotateItemRoutine);
            }

            m_RotateItemRoutine = ChangeItemRotationRoutine();
            StartCoroutine(m_RotateItemRoutine);
        }

        // Handles when to start the translation coroutine.
        // Should only run one at a time.
        private void MoveAnim()
        {
            if (IsCurrentlyInteracting)
            {
                if (!m_isExaminingTranslateItem)
                {
                    StartTranslateCoroutine();
                }
            }
            else
            {
                if (m_isExaminingTranslateItem)
                {
                    StartTranslateCoroutine();
                }
            }
        }

        private void StartTranslateCoroutine()
        {
            if (m_TranslateItemRoutine != null)
            {
                StopCoroutine(m_TranslateItemRoutine);
            }

            m_TranslateItemRoutine = ChangeItemTranslationRoutine();
            StartCoroutine(m_TranslateItemRoutine);
        }
    }

}
