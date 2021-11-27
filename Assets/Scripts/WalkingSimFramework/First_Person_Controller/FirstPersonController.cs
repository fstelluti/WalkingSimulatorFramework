using WalkingSimFramework.CameraSystem;
using WalkingSimFramework.Interactable_System;
using WalkingSimFramework.Scriptable_Objects;
using NaughtyAttributes;
using SmartData.SmartBool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WalkingSimFramework.Helpers;

namespace WalkingSimFramework.First_Person_Controller
{
    /// <summary>
    /// Main First person controller
    /// Modified from VeryHotShark's FPS controller: https://github.com/VeryHotShark/First-Person-Controller-VeryHotShark
    /// Uses the New Input system
    /// </summary>
    public class FirstPersonController : MonoBehaviour
    {
        [Space, Header("Data")]
        [SerializeField] private HeadBobbingData headBobData = null;
        [SerializeField] private InputActionMapData inputActionMapData = null;

        [Space, Header("Locomotion Settings")]
        [SerializeField] private float crouchSpeed = 1f;
        [SerializeField] private float walkSpeed = 2f;
        [SerializeField] private float runSpeed = 3f;
        [SerializeField] private float jumpSpeed = 5f;
        [MinValue(0f), MaxValue(1f)] [SerializeField] private float moveBackwardsSpeedPercent = 0.5f;
        [MinValue(0f), MaxValue(1f)] [SerializeField] private float moveSideSpeedPercent = 0.75f;

        [Space, Header("Run Settings")]
        [MinValue(-1f), MaxValue(1f)] [SerializeField] private float canRunThreshold = 0.8f;
        [SerializeField] private AnimationCurve runTransitionCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [Space, Header("Crouch Settings")]
        [MinValue(0.2f), MaxValue(0.9f)] [SerializeField] private float crouchPercent = 0.6f;
        [SerializeField] private float crouchTransitionDuration = 1f;
        [SerializeField] private AnimationCurve crouchTransitionCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [Space, Header("Landing Settings")]
        [MinValue(0.05f), MaxValue(0.5f)] [SerializeField] private float lowLandAmount = 0.1f;
        [MinValue(0.2f), MaxValue(0.9f)] [SerializeField] private float highLandAmount = 0.6f;
        [SerializeField] private float landTimer = 0.5f;
        [SerializeField] private float landDuration = 1f;
        [SerializeField] private AnimationCurve landCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [Space, Header("Gravity Settings")]
        [SerializeField] private float gravityMultiplier = 2.5f;
        [SerializeField] private float stickToGroundForce = 5f;

        [SerializeField] private LayerMask groundLayer = ~0;
        [MinValue(0f), MaxValue(1f)] [SerializeField] private float rayLength = 0.1f;
        [MinValue(0.01f), MaxValue(1f)] [SerializeField] private float raySphereRadius = 0.1f;

        [Space, Header("Check Wall Settings")]
        [SerializeField] private LayerMask obstacleLayers = ~0;
        [MinValue(0f), MaxValue(1f)] [SerializeField] private float rayObstacleLength = 0.1f;
        [MinValue(0f), MaxValue(1f)] [SerializeField] private float rayObstacleSphereRadius = 0.1f;

        [Space, Header("Smooth Settings")]
        [Range(1f, 100f)] [SerializeField] private float smoothRotateSpeed = 5f;
        [Range(1f, 100f)] [SerializeField] private float smoothInputSpeed = 5f;
        [Range(1f, 100f)] [SerializeField] private float smoothVelocitySpeed = 5f;
        [Range(1f, 100f)] [SerializeField] private float smoothFinalDirectionSpeed = 5f;
        [Range(1f, 100f)] [SerializeField] private float smoothHeadBobSpeed = 5f;

        [Space]
        [InfoBox("Experimental mode: Should smooth player movement to not start fast and not stop fast but it's somehow jerky", EInfoBoxType.Warning)]
        [SerializeField] private bool experimental = false;
        [Tooltip("If set to very high it will stop player immediately after releasing input, otherwise it just another smoothing to our movement to make our player not move fast immediately and not stop immediately")]
        [ShowIf("experimental")] [Range(1f, 100f)] [SerializeField] private float smoothInputMagnitudeSpeed = 5f;

        private CharacterController m_characterController;
        private Transform m_yawTransform;
        private Transform m_camTransform;
        private HeadBobbing m_headBob;
        private CameraController m_cameraController;

        private RaycastHit m_hitInfo;
        private IEnumerator m_CrouchRoutine;
        private IEnumerator m_LandRoutine;

        #region Debug
        [Space]
        [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private Vector2 m_inputVector;
        [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private Vector2 m_smoothInputVector;

        [Space]
        [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private Vector3 m_finalMoveDir;
        [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private Vector3 m_smoothFinalMoveDir;
        [Space]
        [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private Vector3 m_finalMoveVector;

        [Space]
        [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private float m_currentSpeed;
        [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private float m_smoothCurrentSpeed;
        [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private float m_finalSmoothCurrentSpeed;
        [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private float m_walkRunSpeedDifference;

        [Space]
        [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private float m_finalRayLength;
        [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private bool m_hitWall;
        [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private bool m_isGrounded;
        [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private bool m_previouslyGrounded;

        [Space]
        [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private float m_initHeight;
        [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private float m_crouchHeight;
        [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private Vector3 m_initCenter;
        [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private Vector3 m_crouchCenter;
        [Space]
        [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private float m_initCamHeight;
        [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private float m_crouchCamHeight;
        [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private float m_crouchStandHeightDifference;
        [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private float m_crouchRoofAngle;
        [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private bool m_duringCrouchAnimation;
        [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private bool m_duringRunAnimation;
        [Space]
        [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private float m_inAirTimer;

        [Space]
        [BoxGroup("DEBUG")] [ShowIf("experimental")] [SerializeField] [ReadOnly] private float m_inputVectorMagnitude;
        [BoxGroup("DEBUG")] [ShowIf("experimental")] [SerializeField] [ReadOnly] private float m_smoothInputVectorMagnitude;
        #endregion

        // New Input System
        InputActionWrapper InputActionMap;

        // Crouching state - Not related to input
        private bool m_isCrouching;
        
        // Input values
        private Vector2 m_moveVec;
        private float m_runVal;

        // Input clicked states
        private bool m_runClicked;
        private bool m_jumpClicked;

        void Awake()
        {
            InputActionMap = inputActionMapData.InputActionMap;

            InitInteractionActions();
        }

        private void InitInteractionActions()
        {
            // Move player
            InputActionMap.PlayerMoveAction().performed += ctx => m_moveVec = ctx.ReadValue<Vector2>();
            InputActionMap.PlayerMoveAction().canceled += _ => m_moveVec = Vector2.zero;

            // Crouch
            InputActionMap.PlayerCrouchAction().started += _ =>
            {
                // Seems like this needs to be called here instead of in Update, as if it is in Update it will be called multiple times.
                // Calling it here ensures that it is only called on click/press.
                HandleCrouch();
            };

            // Running
            InputActionMap.PlayerRunAction().started += ctx =>
            {
                m_runVal = ctx.ReadValue<float>();
                m_runClicked = true;
            };
            InputActionMap.PlayerRunAction().canceled += _ =>
            {
                m_runVal = 0f;
                m_runClicked = false;
            };

            // Jump
            InputActionMap.PlayerJumpAction().performed += ctx =>
            {
                m_jumpClicked = true;
            };
            InputActionMap.PlayerJumpAction().canceled += _ =>
            {
                m_jumpClicked = false;
            };
        }

        private void Start()
        {
            GetComponents();
            InitVariables();
        }

        private void Update()
        {
            if (m_yawTransform != null)
                RotateTowardsCamera();

            if (m_characterController)
            {
                // Check if Grounded,Wall etc
                CheckIfGrounded();
                CheckIfWall();

                // Apply Smoothing
                SmoothInput();
                SmoothSpeed();
                SmoothDir();

                if (experimental)
                    SmoothInputMagnitude();

                // Calculate Movement
                CalculateMovementDirection();
                CalculateSpeed();
                CalculateFinalMovement();

                // Handle Player Movement, Gravity, Jump, Crouch etc.
                HandleHeadBob();
                HandleCameraSway();
                HandleLanding();
                HandleRunFOV();

                ApplyGravity();
                ApplyMovement();

                m_previouslyGrounded = m_isGrounded;
            }
        }

        #region Initialize Methods  
        protected virtual void GetComponents()
        {
            m_characterController = GetComponent<CharacterController>();
            m_cameraController = GetComponentInChildren<CameraController>();
            m_yawTransform = m_cameraController.transform;
            m_camTransform = GetComponentInChildren<Camera>().transform;
            m_headBob = new HeadBobbing(headBobData, moveBackwardsSpeedPercent, moveSideSpeedPercent);
        }

        protected virtual void InitVariables()
        {
            // Calculate where our character center should be based on height and skin width
            m_characterController.center = new Vector3(0f, m_characterController.height / 2f + m_characterController.skinWidth, 0f);

            m_initCenter = m_characterController.center;
            m_initHeight = m_characterController.height;

            m_crouchHeight = m_initHeight * crouchPercent;
            m_crouchCenter = (m_crouchHeight / 2f + m_characterController.skinWidth) * Vector3.up;

            m_crouchStandHeightDifference = m_initHeight - m_crouchHeight;

            m_initCamHeight = m_yawTransform.localPosition.y;
            m_crouchCamHeight = m_initCamHeight - m_crouchStandHeightDifference;

            // Sphere radius not included. If you want it to be included just decrease by sphere radius at the end of this equation
            m_finalRayLength = rayLength + m_characterController.center.y;

            m_isGrounded = true;
            m_previouslyGrounded = true;

            m_inAirTimer = 0f;
            m_headBob.CurrentStateHeight = m_initCamHeight;

            m_walkRunSpeedDifference = runSpeed - walkSpeed;
        }
        #endregion

        #region Smoothing Methods
        protected virtual void SmoothInput()
        {
            m_inputVector = m_moveVec.normalized;
            m_smoothInputVector = Vector2.Lerp(m_smoothInputVector, m_inputVector, Time.deltaTime * smoothInputSpeed);
        }

        protected virtual void SmoothSpeed()
        {
            m_smoothCurrentSpeed = Mathf.Lerp(m_smoothCurrentSpeed, m_currentSpeed, Time.deltaTime * smoothVelocitySpeed);

            if (m_runVal > 0 && CanRun())
            {
                float _walkRunPercent = Mathf.InverseLerp(walkSpeed, runSpeed, m_smoothCurrentSpeed);
                m_finalSmoothCurrentSpeed = runTransitionCurve.Evaluate(_walkRunPercent) * m_walkRunSpeedDifference + walkSpeed;
            }
            else
            {
                m_finalSmoothCurrentSpeed = m_smoothCurrentSpeed;
            }
        }

        protected virtual void SmoothDir()
        {

            m_smoothFinalMoveDir = Vector3.Lerp(m_smoothFinalMoveDir, m_finalMoveDir, Time.deltaTime * smoothFinalDirectionSpeed);
        }

        protected virtual void SmoothInputMagnitude()
        {
            m_inputVectorMagnitude = m_inputVector.magnitude;
            m_smoothInputVectorMagnitude = Mathf.Lerp(m_smoothInputVectorMagnitude, m_inputVectorMagnitude, Time.deltaTime * smoothInputMagnitudeSpeed);
        }
        #endregion

        #region Locomotion Calculation Methods
        protected virtual void CheckIfGrounded()
        {
            Vector3 _origin = transform.position + m_characterController.center;

            bool _hitGround = Physics.SphereCast(_origin, raySphereRadius, Vector3.down, out m_hitInfo, m_finalRayLength, groundLayer);

            m_isGrounded = _hitGround ? true : false;
        }

        protected virtual void CheckIfWall()
        {

            Vector3 _origin = transform.position + m_characterController.center;
            RaycastHit _wallInfo;

            bool _hitWall = false;
            if ((Math.Abs(m_moveVec.x) > 0 || Math.Abs(m_moveVec.y) > 0)  && m_finalMoveDir.sqrMagnitude > 0)
                _hitWall = Physics.SphereCast(_origin, rayObstacleSphereRadius, m_finalMoveDir, out _wallInfo, rayObstacleLength, obstacleLayers);

            m_hitWall = _hitWall ? true : false;
        }

        protected virtual bool CheckIfRoof()
        {
            Vector3 _origin = transform.position;
            RaycastHit _roofInfo;

            // Raycast to check if anything is directly above first.
            bool _hitRoofUpwards = Physics.SphereCast(_origin, rayObstacleSphereRadius, Vector3.up, out _roofInfo, m_initHeight, obstacleLayers);

            if (_hitRoofUpwards)
                return true;

            // If moving, also need to raycast at an angle relative to the direction of movement, so that cannot uncrouch (and clip) into a small space.
            // Since only checking the upwards direction isn't necessarily enough.
            // But also need to make sure we are not running into a wall (since _hitRoofAtAngle would still hit something)
            bool _hitRoofAtAngle = false;
            
            if (!m_hitWall)
            {
                if ((Math.Abs(m_moveVec.x) > 0 || Math.Abs(m_moveVec.y) > 0) && m_finalMoveDir.sqrMagnitude > 0)
                {
                    m_crouchRoofAngle = -80f;

                    Vector3 _roofAngleDir = Quaternion.AngleAxis(m_crouchRoofAngle, transform.right) * m_finalMoveDir;
                    _hitRoofAtAngle = Physics.SphereCast(_origin, rayObstacleSphereRadius, _roofAngleDir, out _roofInfo, m_initHeight, obstacleLayers);
                }
            }

            return _hitRoofAtAngle;
        }

        protected virtual bool CanRun()
        {
            Vector3 _normalizedDir = Vector3.zero;

            if (m_smoothFinalMoveDir != Vector3.zero)
                _normalizedDir = m_smoothFinalMoveDir.normalized;

            float _dot = Vector3.Dot(transform.forward, _normalizedDir);

            return _dot >= canRunThreshold && !m_isCrouching ? true : false;
        }

        protected virtual void CalculateMovementDirection()
        {

            Vector3 _vDir = transform.forward * m_smoothInputVector.y;
            Vector3 _hDir = transform.right * m_smoothInputVector.x;

            Vector3 _desiredDir = _vDir + _hDir;
            Vector3 _flattenDir = FlattenVectorOnSlopes(_desiredDir);

            m_finalMoveDir = _flattenDir;
        }

        protected virtual Vector3 FlattenVectorOnSlopes(Vector3 _vectorToFlat)
        {
            if (m_isGrounded)
                _vectorToFlat = Vector3.ProjectOnPlane(_vectorToFlat, m_hitInfo.normal);

            return _vectorToFlat;
        }

        protected virtual void CalculateSpeed()
        {
            m_currentSpeed = m_runVal > 0 && CanRun() ? runSpeed : walkSpeed;
            m_currentSpeed = m_isCrouching ? crouchSpeed : m_currentSpeed;
            m_currentSpeed = !(Math.Abs(m_moveVec.x) > 0 || Math.Abs(m_moveVec.y) > 0) ? 0f : m_currentSpeed;
            m_currentSpeed = m_moveVec.y == -1 ? m_currentSpeed * moveBackwardsSpeedPercent : m_currentSpeed;
            m_currentSpeed = m_moveVec.x != 0 && m_moveVec.y == 0 ? m_currentSpeed * moveSideSpeedPercent : m_currentSpeed;
        }

        protected virtual void CalculateFinalMovement()
        {
            float _smoothInputVectorMagnitude = experimental ? m_smoothInputVectorMagnitude : 1f;

            Vector3 _finalVector = m_smoothFinalMoveDir * m_finalSmoothCurrentSpeed * _smoothInputVectorMagnitude;

            // We have to assign individually in order to make our character jump properly because before it was overwriting Y value and that's why it was jerky now we are adding to Y value and it's working
            m_finalMoveVector.x = _finalVector.x;
            m_finalMoveVector.z = _finalVector.z;

            if (m_characterController.isGrounded) // Thanks to this check we are not applying extra y velocity when in air so jump will be consistent
                m_finalMoveVector.y += _finalVector.y; //so this makes our player go in forward dir using slope normal but when jumping this is making it go higher so this is weird
        }
        #endregion

        #region Crouching Methods
        protected virtual void HandleCrouch()
        {
            if (m_isGrounded)
                InvokeCrouchRoutine();
        }

        protected virtual void InvokeCrouchRoutine()
        {
            if (m_isCrouching) 
                if (CheckIfRoof())
                    return;


            if (m_LandRoutine != null)
                StopCoroutine(m_LandRoutine);

            if (m_CrouchRoutine != null)
                StopCoroutine(m_CrouchRoutine);

            m_CrouchRoutine = CrouchRoutine();
            StartCoroutine(m_CrouchRoutine);
        }

        protected virtual IEnumerator CrouchRoutine()
        {
            m_duringCrouchAnimation = true;

            float _percent = 0f;
            float _smoothPercent = 0f;
            float _speed = 1f / crouchTransitionDuration;

            float _currentHeight = m_characterController.height;
            Vector3 _currentCenter = m_characterController.center;

            float _desiredHeight = m_isCrouching ? m_initHeight : m_crouchHeight;
            Vector3 _desiredCenter = m_isCrouching ? m_initCenter : m_crouchCenter;

            Vector3 _camPos = m_yawTransform.localPosition;
            float _camCurrentHeight = _camPos.y;
            float _camDesiredHeight = m_isCrouching ? m_initCamHeight : m_crouchCamHeight;

            // Still need to save crouching state
            m_isCrouching = !m_isCrouching;

            m_headBob.CurrentStateHeight = m_isCrouching ? m_crouchCamHeight : m_initCamHeight;

            while (_percent < 1f)
            {
                _percent += Time.deltaTime * _speed;
                _smoothPercent = crouchTransitionCurve.Evaluate(_percent);

                m_characterController.height = Mathf.Lerp(_currentHeight, _desiredHeight, _smoothPercent);
                m_characterController.center = Vector3.Lerp(_currentCenter, _desiredCenter, _smoothPercent);

                _camPos.y = Mathf.Lerp(_camCurrentHeight, _camDesiredHeight, _smoothPercent);
                m_yawTransform.localPosition = _camPos;

                yield return null;
            }

            m_duringCrouchAnimation = false;
        }

        #endregion

        #region Landing Methods
        protected virtual void HandleLanding()
        {
            if (!m_previouslyGrounded && m_isGrounded)
            {
                InvokeLandingRoutine();
            }
        }

        protected virtual void InvokeLandingRoutine()
        {
            if (m_LandRoutine != null)
                StopCoroutine(m_LandRoutine);

            m_LandRoutine = LandingRoutine();
            StartCoroutine(m_LandRoutine);
        }

        protected virtual IEnumerator LandingRoutine()
        {
            float _percent = 0f;
            float _landAmount = 0f;

            float _speed = 1f / landDuration;

            Vector3 _localPos = m_yawTransform.localPosition;
            float _initLandHeight = _localPos.y;

            _landAmount = m_inAirTimer > landTimer ? highLandAmount : lowLandAmount;

            while (_percent < 1f)
            {
                _percent += Time.deltaTime * _speed;
                float _desiredY = landCurve.Evaluate(_percent) * _landAmount;

                _localPos.y = _initLandHeight + _desiredY;
                m_yawTransform.localPosition = _localPos;

                yield return null;
            }
        }
        #endregion

        #region Locomotion Apply Methods

        protected virtual void HandleHeadBob()
        {
            if ((Math.Abs(m_moveVec.x) > 0 || Math.Abs(m_moveVec.y) > 0) && m_isGrounded && !m_hitWall)
            {
                if (!m_duringCrouchAnimation) // we want to make our head bob only if we are moving and not during crouch routine
                {
                    m_headBob.ScrollHeadBob(m_runVal > 0 && CanRun(), m_isCrouching, m_moveVec);
                    m_yawTransform.localPosition = Vector3.Lerp(m_yawTransform.localPosition, (Vector3.up * m_headBob.CurrentStateHeight) + m_headBob.FinalOffset, Time.deltaTime * smoothHeadBobSpeed);
                }
            }
            else // if we are not moving or we are not grounded
            {
                if (!m_headBob.Resetted)
                {
                    m_headBob.ResetHeadBob();
                }

                if (!m_duringCrouchAnimation) // we want to reset our head bob only if we are standing still and not during crouch routine
                    m_yawTransform.localPosition = Vector3.Lerp(m_yawTransform.localPosition, new Vector3(0f, m_headBob.CurrentStateHeight, 0f), Time.deltaTime * smoothHeadBobSpeed);
            }
        }

        protected virtual void HandleCameraSway()
        {
            m_cameraController.HandleSway(m_smoothInputVector, m_moveVec.x);
        }

        protected virtual void HandleRunFOV()
        {
            // If we are moving, on the ground, and not hitting a wall the apply the new FOV
            if ((Math.Abs(m_moveVec.x) > 0 || Math.Abs(m_moveVec.y) > 0) && m_isGrounded && !m_hitWall)
            {
                // If holding or clicked the run button, can run, and not in the middle of the run animation (i.e. haven't change FOV), then change the FOV to the new one
                if (m_runClicked && CanRun() && !m_duringRunAnimation)
                {
                    m_duringRunAnimation = true;
                    m_cameraController.ChangeRunFOV(false);
                }
            }

            // If button to run has been released, there is no movement, or we hit a wall, then return to original FOV
            if (!m_runClicked || !(Math.Abs(m_moveVec.x) > 0 || Math.Abs(m_moveVec.y) > 0) || m_hitWall)
            {
                if (m_duringRunAnimation)
                {
                    m_duringRunAnimation = false;
                    m_cameraController.ChangeRunFOV(true);
                }
            }
        }
        protected virtual void HandleJump()
        {
            if (m_jumpClicked && !(m_isCrouching))
            {
                m_finalMoveVector.y = jumpSpeed;

                m_previouslyGrounded = true;
                m_isGrounded = false;
            }
        }
        protected virtual void ApplyGravity()
        {
            if (m_characterController.isGrounded) // if we would use our own m_isGrounded it would not work that well, this one is more precise
            {
                m_inAirTimer = 0f;
                m_finalMoveVector.y = -stickToGroundForce;

                HandleJump();
            }
            else
            {
                m_inAirTimer += Time.deltaTime;
                m_finalMoveVector += Physics.gravity * gravityMultiplier * Time.deltaTime;
            }
        }

        protected virtual void ApplyMovement()
        {
            m_characterController.Move(m_finalMoveVector * Time.deltaTime);
        }

        protected virtual void RotateTowardsCamera()
        {
            Quaternion _currentRot = transform.rotation;
            Quaternion _desiredRot = m_yawTransform.rotation;

            transform.rotation = Quaternion.Slerp(_currentRot, _desiredRot, Time.deltaTime * smoothRotateSpeed);
        }
        #endregion

        // Must manually enable/disable actions
        private void OnEnable()
        {
            InputActionMap.Enable();
        }

        private void OnDisable()
        {
            InputActionMap.Disable();
        }
    }
}
