using WalkingSimFramework.Scriptable_Objects;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WalkingSimFramework.CameraSystem
{
    /// <summary>
    /// Camera Controller
    /// Modified from VeryHotShark's FPS controller: https://github.com/VeryHotShark/First-Person-Controller-VeryHotShark
    /// Uses the New Input system
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        //Uses the WalkingSimActionMap, same as in the parent scripts
        [Space, Header("Data")]
        [SerializeField] private InputActionMapData inputActionMapData = null;

        [Space, Header("Custom Classes")]
        [SerializeField] private CameraZoom cameraZoom = null;
        [SerializeField] private CameraSwaying cameraSway = null;

        [Space, Header("Look Settings")]
        [SerializeField] private Vector2 sensitivity = Vector2.zero;
        [SerializeField] private Vector2 smoothAmount = Vector2.zero;
        [SerializeField] [MinMaxSlider(-90f, 90f)] private Vector2 lookAngleMinMax = Vector2.zero;

        private float m_yaw;
        private float m_pitch;

        private float m_desiredYaw;
        private float m_desiredPitch;

        private Transform m_pitchTranform;
        private Camera m_cam;

        // Input map
        WalkingSimActionMap m_inputActionMap;

        // Input values
        private Vector2 m_lookVec;

        void Awake()
        {
            m_inputActionMap = inputActionMapData.InputActionMap;

            InitInputActions();

            GetComponents();
            InitValues();
            InitComponents();
            ChangeCursorState();
        }

        void LateUpdate()
        {
            // This is done in late update because the camera should move after the cursor does (like following a character, need to know where to go first)
            CalculateRotation();
            SmoothRotation();
            ApplyRotation();
            HandleZoom();
        }

        void InitInputActions()
        {
            // Look around
            m_inputActionMap.Player.Look.performed += ctx => m_lookVec = ctx.ReadValue<Vector2>();
            m_inputActionMap.Player.Look.canceled += _ => m_lookVec = Vector2.zero;
        }

        void GetComponents()
        {
            // Assume the first child is the camera pivot
            m_pitchTranform = transform.GetChild(0).transform;
            m_cam = GetComponentInChildren<Camera>();
        }

        void InitValues()
        {
            m_yaw = transform.eulerAngles.y;
            m_desiredYaw = m_yaw;
        }

        void InitComponents()
        {
            cameraZoom.Init(m_cam, m_inputActionMap.Player.Zoom);
            cameraSway.Init(m_cam.transform);
        }

        void CalculateRotation()
        {
            m_desiredYaw += m_lookVec.x * sensitivity.x * Time.deltaTime;
            m_desiredPitch -= m_lookVec.y * sensitivity.y * Time.deltaTime;

            m_desiredPitch = Mathf.Clamp(m_desiredPitch, lookAngleMinMax.x, lookAngleMinMax.y);
        }

        void SmoothRotation()
        {
            m_yaw = Mathf.Lerp(m_yaw, m_desiredYaw, smoothAmount.x * Time.deltaTime);
            m_pitch = Mathf.Lerp(m_pitch, m_desiredPitch, smoothAmount.y * Time.deltaTime);
        }

        void ApplyRotation()
        {
            // For a FPS controller, the pitch and yaw need to be separated
            // Yaw = move whole player controller
            // Pitch = only want to move camera, otherwise entire player will rotate 'forwards'
            transform.eulerAngles = new Vector3(0, m_yaw, 0f);
            m_pitchTranform.localEulerAngles = new Vector3(m_pitch, 0f, 0f);
        }

        public void HandleSway(Vector3 _inputVector, float _rawXInput)
        {
            cameraSway.SwayPlayer(_inputVector, _rawXInput);
        }

        void HandleZoom()
        {
            if (cameraZoom.m_isZoomingIn || cameraZoom.m_isZoomingOut)
            {
                cameraZoom.ChangeFOV(this);
            }
        }

        public void ChangeRunFOV(bool _returning)
        {
            cameraZoom.ChangeRunFOV(_returning, this);
        }

        void ChangeCursorState()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Must manually enable/disable actions
        private void OnEnable()
        {
            m_inputActionMap.Enable();
        }

        private void OnDisable()
        {
            m_inputActionMap.Disable();
        }

    }
}
