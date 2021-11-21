using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WalkingSimFramework.CameraSystem
{
    /// <summary>
    /// Camera Zoom settings
    /// Modified from VeryHotShark's FPS controller: https://github.com/VeryHotShark/First-Person-Controller-VeryHotShark
    /// Uses the New Input system
    /// </summary>
    [System.Serializable]
    public class CameraZoom
    {
        [Space, Header("Zoom Settings")]
        [Range(20f, 60f)] [SerializeField] private float zoomFOV = 20f;
        [SerializeField] private AnimationCurve zoomCurve = new AnimationCurve();
        [Min(0.1f)][SerializeField] private float zoomTransitionDuration;

        [Space, Header("Run Settings")]
        [Range(60f, 100f)] [SerializeField] private float runFOV = 60f;
        [SerializeField] private AnimationCurve runCurve = new AnimationCurve();
        [SerializeField] private float runTransitionDuration = 0f;
        [Min(0.1f)][SerializeField] private float runReturnTransitionDuration;

        private float m_initFOV;
        private InputAction m_camZoomAction;

        private bool m_running;

        private float m_zoomVal;

        // Used for clicked and release actions
        internal bool m_isZoomingIn;
        internal bool m_isZoomingOut;

        private Camera m_cam;

        private IEnumerator m_ChangeFOVRoutine;
        private IEnumerator m_ChangeRunFOVRoutine;

        public void Init(Camera _cam, InputAction _zoomAction)
        {
            m_camZoomAction = _zoomAction;

            InitInputActions();

            m_cam = _cam;
            m_initFOV = m_cam.fieldOfView;
        }

        void InitInputActions()
        {
            // Zoom in
            m_camZoomAction.performed += ctx =>
            {
                m_zoomVal = ctx.ReadValue<float>();
                m_isZoomingIn = true;
            };

            // Zoom out
            m_camZoomAction.canceled += _ =>
            {
                m_zoomVal = 0f;
                m_isZoomingOut = true;
            };
        }

        public void ChangeFOV(MonoBehaviour _mono)
        {
            // If not running, or zooming in/out
            if ((!m_isZoomingIn && !m_isZoomingOut) || m_running)
            {
                return;
            }

            if (m_ChangeRunFOVRoutine != null)
                _mono.StopCoroutine(m_ChangeRunFOVRoutine);

            if (m_ChangeFOVRoutine != null)
                _mono.StopCoroutine(m_ChangeFOVRoutine);

            m_ChangeFOVRoutine = ChangeFOVRoutine();
            _mono.StartCoroutine(m_ChangeFOVRoutine);
        }

        IEnumerator ChangeFOVRoutine()
        {
            float _percent = 0f;
            float _smoothPercent = 0f;

            float _speed = 1f / zoomTransitionDuration;

            float _currentFOV = m_cam.fieldOfView;
            float _targetFOV = m_zoomVal > 0 ? zoomFOV : m_initFOV;

            m_isZoomingIn = false;
            m_isZoomingOut = false;

            while (_percent < 1f)
            {
                _percent += Time.deltaTime * _speed;
                _smoothPercent = zoomCurve.Evaluate(_percent);
                m_cam.fieldOfView = Mathf.Lerp(_currentFOV, _targetFOV, _smoothPercent);

                yield return null;
            }
        }

        public void ChangeRunFOV(bool _returning, MonoBehaviour _mono)
        {
            if (m_ChangeFOVRoutine != null)
                _mono.StopCoroutine(m_ChangeFOVRoutine);

            if (m_ChangeRunFOVRoutine != null)
                _mono.StopCoroutine(m_ChangeRunFOVRoutine);

            m_ChangeRunFOVRoutine = ChangeRunFOVRoutine(_returning);
            _mono.StartCoroutine(m_ChangeRunFOVRoutine);
        }

        IEnumerator ChangeRunFOVRoutine(bool _returning)
        {
            float _percent = 0f;
            float _smoothPercent = 0f;

            float _duration = _returning ? runReturnTransitionDuration : runTransitionDuration;
            float _speed = 1f / _duration;

            float _currentFOV = m_cam.fieldOfView;
            float _targetFOV = _returning ? m_initFOV : runFOV;

            m_running = !_returning;

            while (_percent < 1f)
            {
                _percent += Time.deltaTime * _speed;
                _smoothPercent = runCurve.Evaluate(_percent);
                m_cam.fieldOfView = Mathf.Lerp(_currentFOV, _targetFOV, _smoothPercent);

                yield return null;
            }
        }
    }
}
