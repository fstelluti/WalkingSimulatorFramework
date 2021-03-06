using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WalkingSimFramework.CameraSystem
{
    /// <summary>
    /// Camera swaying when moving from side to side
    /// Credit to VeryHotShark's FPS controller: https://github.com/VeryHotShark/First-Person-Controller-VeryHotShark
    /// </summary>
    [System.Serializable]
    public class CameraSwaying
    {
        [Space, Header("Sway Settings")]
        [SerializeField] private float swayAmount = 0f;
        [SerializeField] private float swaySpeed = 0f;
        [SerializeField] private float returnSpeed = 0f;
        [SerializeField] private float changeDirectionMultiplier = 0f;

        [SerializeField] private AnimationCurve swayCurve = new AnimationCurve();

        private Transform m_camTransform;
        private float m_scrollSpeed;

        private float m_xAmountThisFrame;
        private float m_xAmountPreviousFrame;

        private bool m_diffrentDirection;

        public void Init(Transform _cam)
        {
            m_camTransform = _cam;
        }

        public void SwayPlayer(Vector3 _inputVector, float _rawXInput)
        {
            float _xAmount = _inputVector.x;

            m_xAmountThisFrame = _rawXInput;

            if (_rawXInput != 0f) // if we have some input
            {
                if (m_xAmountThisFrame != m_xAmountPreviousFrame && m_xAmountPreviousFrame != 0) // if our previous dir is not equal to current one and the previous one was not idle
                    m_diffrentDirection = true;

                // then we multiply our scroll so when changing direction it will sway to the other direction faster
                float _speedMultiplier = m_diffrentDirection ? changeDirectionMultiplier : 1f;
                m_scrollSpeed += (_xAmount * swaySpeed * Time.deltaTime * _speedMultiplier);
            }
            else // if we are not moving so there is no input
            {
                if (m_xAmountThisFrame == m_xAmountPreviousFrame) // check if our previous dir equals current dir
                    m_diffrentDirection = false; // if yes we want to reset this bool so basically it can be used correctly once we move again

                m_scrollSpeed = Mathf.Lerp(m_scrollSpeed, 0f, Time.deltaTime * returnSpeed);
            }

            m_scrollSpeed = Mathf.Clamp(m_scrollSpeed, -1f, 1f);

            float _swayFinalAmount;

            if (m_scrollSpeed < 0f)
                _swayFinalAmount = -swayCurve.Evaluate(m_scrollSpeed) * -swayAmount;
            else
                _swayFinalAmount = swayCurve.Evaluate(m_scrollSpeed) * -swayAmount;

            Vector3 _swayVector;
            _swayVector.z = _swayFinalAmount;

            m_camTransform.localEulerAngles = new Vector3(m_camTransform.localEulerAngles.x, m_camTransform.localEulerAngles.y, _swayVector.z);

            m_xAmountPreviousFrame = m_xAmountThisFrame;
        }
    }
}
