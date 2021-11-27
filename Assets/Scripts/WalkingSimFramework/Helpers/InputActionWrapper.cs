using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace WalkingSimFramework.Helpers
{
    /// <summary>
    /// Encapsulates the details of the current input action asset
    /// </summary>
    public class InputActionWrapper
    {
        // For now, this is hardcoded
        private WalkingSimActionMap m_WalkingSimActionMap;

        public InputActionWrapper()
        {
            m_WalkingSimActionMap = new WalkingSimActionMap();
        }

        /// <summary>
        /// Basic movement inputs (and for now, fire)
        /// </summary>
        public List<InputAction> GetBasicInputs()
        {
            List<InputAction> _basicActions = new List<InputAction>();

            _basicActions.Add(m_WalkingSimActionMap.Player.Move);
            _basicActions.Add(m_WalkingSimActionMap.Player.Jump);
            _basicActions.Add(m_WalkingSimActionMap.Player.Fire);
            _basicActions.Add(m_WalkingSimActionMap.Player.Crouch);
            _basicActions.Add(m_WalkingSimActionMap.Player.Run);
            _basicActions.Add(m_WalkingSimActionMap.Player.Look);

            return _basicActions;
        }

        // Player Inputs
        
        public InputAction PlayerMoveAction()
        {
            return m_WalkingSimActionMap.Player.Move;
        }

        public InputAction PlayerMoveExamineAction()
        {
            return m_WalkingSimActionMap.Player.MoveExamine;
        }

        public InputAction PlayerLookAction()
        {
            return m_WalkingSimActionMap.Player.Look;
        }

        public InputAction PlayerZoomAction()
        {
            return m_WalkingSimActionMap.Player.Zoom;
        }

        public InputAction PlayerCrouchAction()
        {
            return m_WalkingSimActionMap.Player.Crouch;
        }

        public InputAction PlayerRunAction()
        {
            return m_WalkingSimActionMap.Player.Run;
        }

        public InputAction PlayerJumpAction()
        {
            return m_WalkingSimActionMap.Player.Jump;
        }

        public InputAction PlayerInteractAction()
        {
            return m_WalkingSimActionMap.Player.Interact;
        }

        // UI inputs

        public InputAction UIOpenAction()
        {
            return m_WalkingSimActionMap.UI.Open;
        }

        public void Enable()
        {
            m_WalkingSimActionMap.Enable();
        }

        public void Disable()
        {
            m_WalkingSimActionMap.Disable();
        }
    }
}
