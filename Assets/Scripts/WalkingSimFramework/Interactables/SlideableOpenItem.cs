using WalkingSimFramework.Interactable_System;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WalkingSimFramework.Interactables
{
    /// <summary>
    /// Used for items that can slide forward and backwards, such as drawers or sliding doors
    /// </summary>
    public class SlideableOpenItem : OpenableBase
    {
        [Space, Header("Slider Settings")]
        [Required("Transform required to indicate the ending point of the sliding direction.")]
        [SerializeField] private Transform endingTransform;

        [Required("Transform required to indicate the starting point of the sliding direction.")]
        [ValidateInput("IsChildObject", "Transform must be a child of the current object")]
        [SerializeField] private Transform startingTransform;

        IEnumerator m_OpeningRoutine;

        // Position when the item is 100% closed
        Vector3 m_originalStartingPosition;

        protected override void Awake()
        {
            base.Awake();

            // Get the parent's position. This will be the starting point even if the item is partially 'opened'
            m_originalStartingPosition = transform.parent.position;
        }

        // Inspector Validation method
        private bool IsChildObject(Transform _transform)
        {
            return _transform != null && _transform.IsChildOf(transform);
        }

        private IEnumerator SlidingRoutine()
        {
            float _percent = 0f;
            float _smoothPercent = 0f;
            float _speed = 1f / openingTransitionDuration;

            Vector3 _currentPosition = transform.position;

            // Want the bounds (defined by startingTransform) to stop when reaching the end position (endingTransform).
            // Don't want the center of the mesh to stop at the endingTransform.
            Vector3 _desiredPosition = m_shouldCloseNext ? (endingTransform.position - startingTransform.position) + _currentPosition : m_originalStartingPosition;

            m_shouldCloseNext = !m_shouldCloseNext;

            while (!m_pauseCoroutine && _percent < 1f)
            {
                _percent += Time.deltaTime * _speed;
                _smoothPercent = openingCurve.Evaluate(_percent);

                transform.position = Vector3.Lerp(_currentPosition, _desiredPosition, _smoothPercent);

                yield return null;
            }
        }

        public override void OpenItemAnimation()
        {
            base.OpenItemAnimation();

            if (!IsLocked)
            {
                if (m_OpeningRoutine != null)
                {
                    StopCoroutine(m_OpeningRoutine);
                }

                m_OpeningRoutine = SlidingRoutine();
                StartCoroutine(m_OpeningRoutine);
            }
        }
    }
}
