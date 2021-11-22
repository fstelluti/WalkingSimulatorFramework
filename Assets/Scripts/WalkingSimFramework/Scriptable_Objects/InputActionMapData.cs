using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WalkingSimFramework.Scriptable_Objects
{
    /// <summary>
    /// Data class that contains a shared instance of the InputActionMap for this project
    /// </summary>
    [CreateAssetMenu(fileName = "Input Action Map Data", menuName = "WalkingSimFramework/InputActionMapData")]
    public class InputActionMapData : ScriptableObject
    {
        private WalkingSimActionMap m_InputActionMap;

        public WalkingSimActionMap InputActionMap { 
            get {
                if (m_InputActionMap == null)
                {
                    m_InputActionMap = new WalkingSimActionMap();
                }

                return m_InputActionMap;
            }
        }
    }
}
