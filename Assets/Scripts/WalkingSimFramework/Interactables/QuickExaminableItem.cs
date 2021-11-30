using WalkingSimFramework.Interactable_System;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WalkingSimFramework.Interactables
{
    /// <summary>
    /// Used for examining items without any animations to move it in front of the player and back to the original position.
    /// Optionally, will still have an animation to move the item in the player's view, but starting off screen.
    /// </summary>
    public class QuickExaminableItem : ExaminableItemBase
    {
        [Space, Header("Quick Translation Animation Settings")]
        [Tooltip("How fast item should move from the bottom of the screen to the center of the camera view")]
        [MinValue(1.0f)] [SerializeField] private float movementToCameraAnimSpeed = 3.0f;
        [Tooltip("Enables movement animation. Disable if want instantaneous selection")]
        [SerializeField] private bool enableAnimations = true;

        float m_waitTimeForCoroutine = 0.2f;
        WaitForSeconds m_waitForCoroutine;

        // Animation 
        IEnumerator m_TranslateItemRoutine;

        bool m_isExaminingTranslateItem;
        bool m_isTranslateCoroutineRunning;

        protected override void Awake()
        {
            base.Awake();

            m_waitForCoroutine = new WaitForSeconds(m_waitTimeForCoroutine);
        }

        public override void OnInteract()
        {
            base.OnInteract();

            if (!m_isTranslateCoroutineRunning)
            {
                IsCurrentlyInteracting = !IsCurrentlyInteracting;

                OnUseOrPickUpItem();

                // Enable/Disable needed input actions
                ToggleInputs(IsCurrentlyInteracting);

                if (IsCurrentlyInteracting)
                {
                    ResetAppearance();
                    m_totalAngle = 0f;
                }
                else
                {
                    // Just instantaniously place the object back to the original position
                    transform.position = m_originalPosition;
                    transform.rotation = m_originalRotation;

                    // Re-apply any effects
                    OnHoverOver();

                    return;
                }

                // Rotation
                Vector3 _targetDirection = m_player_cam.transform.forward * -1.0f;
                float _rotationCorrection = 85.0f;

                Quaternion _targetRotation = Quaternion.LookRotation(_targetDirection);
                _targetRotation *= Quaternion.Euler(_rotationCorrection, 0f, 0);

                transform.rotation = _targetRotation;

                // Position
                Vector3 _targetPosition = GetCameraTargetPosition();
                transform.position = _targetPosition;

                // Move item 'out of the view' first
                transform.Translate(Vector3.forward);  

                // Adjust rotation after final positioning if not using the reading orientation. So that the item comes in from the bottom of the screen.
                if(!useReadingOrientation)
                {
                    _targetRotation *= Quaternion.Euler(-_rotationCorrection, 0, 0);
                    transform.rotation = _targetRotation;
                }

            }
        }

        private void Update()
        {
            // Block from rotating item unless examining it
            if (IsCurrentlyInteracting && !m_isTranslateCoroutineRunning)
            {
                RotateExaminedItem();
            }

            MoveAnim();
        }

        /// <summary>
        /// Coroutine used to animate the translation of the object. Will move the object in front of the camera starting at a point out of view.
        /// </summary>
        private IEnumerator ChangeItemTranslationRoutine()
        {
            m_isTranslateCoroutineRunning = true;

            // Wait a bit to give time for the FPS controller to stop, so that the item can be centered properly in front of the camera. 
            yield return m_waitForCoroutine;

            // Logic to toggle positions
            Vector3 _targetPosition = GetCameraTargetPosition();

            if (!enableAnimations)
            {
                // Set position directly, after waiting a bit for the final position
                transform.position = _targetPosition;

                m_initXAxis = transform.right;

                m_isTranslateCoroutineRunning = false;
                yield break;
            }

            while (transform.position != _targetPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * movementToCameraAnimSpeed);
                yield return null;
            }

            // Save the rotation axis so that it is always the same when rotating the object manually.
            // Needs to be set after final position has been reached.
            m_initXAxis = transform.right;

            m_isTranslateCoroutineRunning = false;
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
                    m_isExaminingTranslateItem = true;
                }
            }
            else
            {
                if(m_isExaminingTranslateItem)
                {
                    m_isExaminingTranslateItem = false;
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
