using UnityEngine;
using UnityEngine.InputSystem;
using WalkingSimFramework.Helpers;

namespace WalkingSimFramework.Scriptable_Objects
{
    /// <summary>
    /// Data class that contains a shared instance of the InputActionMap for this project
    /// </summary>
    [CreateAssetMenu(fileName = "Input Action Map Data", menuName = "WalkingSimFramework/InputActionMapData")]
    public class InputActionMapData : ScriptableObject
    {
        private InputActionWrapper m_InputActionMap;

        public InputActionWrapper InputActionMap { 
            get {
                if (m_InputActionMap == null)
                {
                    m_InputActionMap = new InputActionWrapper();
                }

                return m_InputActionMap;
            }
        }
    }
}
