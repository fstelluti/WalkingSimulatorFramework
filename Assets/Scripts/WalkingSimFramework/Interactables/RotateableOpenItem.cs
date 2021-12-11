using WalkingSimFramework.Interactable_System;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WalkingSimFramework.Interactables
{
    /// <summary>
    /// Used for items that can be 'opened' by rotating them, such as most regular doors.
    /// But can be used for anything that requires a rotation around its pivot point for a certain angle.
    /// </summary>
    public class RotateableOpenItem : OpenableBase
    {
        [Space, Header("Rotation Opening Settings")]
        
        [Required("Transform required to indicate the pivot point for the rotation.")]
        [ValidateInput("IsParentObject", "Transform must be a parent of the current object")]
        [SerializeField] private Transform pivotTransform;

        [Tooltip("How far to rotate the item when opening")]
        [MinValue(-360f), MaxValue(360f)] [SerializeField] private float totalOpenAngle = 90f;

        [Dropdown("GetRotationAxis")]
        [SerializeField] private Vector3 rotationAxis;

        private IEnumerator m_RotationOpeningRoutine;

        private Quaternion m_startingRotation;

        // Inspector Validation method
        private bool IsParentObject(Transform _transform)
        {
            return _transform != null && _transform == transform.parent;
        }

        // Uses the parent as the pivot axis
        private DropdownList<Vector3> GetRotationAxis()
        {
            if(pivotTransform == null)
            {
                return new DropdownList<Vector3>()
                {
                    { "NoDirectionPossible",   Vector3.zero }
                };
            }

            return new DropdownList<Vector3>()
            {
                { "X",   pivotTransform.right },
                { "Y",   pivotTransform.up },
                { "Z",   pivotTransform.forward }
            };
        }

        protected override void Awake()
        {
            base.Awake();

            m_startingRotation = transform.rotation;
        }

        private IEnumerator RotationOpenRoutine()
        {
            float _percent = 0f;
            float _smoothPercent = 0f;
            float _speed = 1f / openingTransitionDuration;

            Quaternion _targetRot = m_shouldCloseNext ? Quaternion.AngleAxis(totalOpenAngle, rotationAxis) : m_startingRotation;

            m_shouldCloseNext = !m_shouldCloseNext;

            while (!m_pauseCoroutine && _percent < 1f)
            {
                _percent += Time.deltaTime * _speed;
                _smoothPercent = openingCurve.Evaluate(_percent);

                // Rotates around the parent pivot point
                pivotTransform.rotation = Quaternion.RotateTowards(pivotTransform.rotation, _targetRot, _smoothPercent);

                yield return null;
            }
        }

        public override void OpenItemAnimation()
        {
            base.OpenItemAnimation();

            if (!IsLocked)
            {
                if (m_RotationOpeningRoutine != null)
                {
                    StopCoroutine(m_RotationOpeningRoutine);
                }

                m_RotationOpeningRoutine = RotationOpenRoutine();
                StartCoroutine(m_RotationOpeningRoutine);
            }
        }
    }
}
