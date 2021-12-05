using SmartData.SmartString.Data;
using UnityEngine;

namespace WalkingSimFramework.Helpers.Highlight
{
    /// <summary>
    /// Class used to switch toggle item highlighting (using URP).
    /// </summary>
    [System.Serializable]
    public class HighlightItem : IInteractableHighlight
    {
        [SerializeField] StringConst highlightLayer;

        int m_highlightLayer = -1;
        int m_defaultLayer = -1;

        GameObject m_itemObject;

        public void InitInstance(GameObject _gameObject)
        {
            m_itemObject = _gameObject;
            m_defaultLayer = _gameObject.layer;
            if(highlightLayer != null)
            {
                m_highlightLayer = LayerMask.NameToLayer(highlightLayer.value);
            }
        }

        public void OnItemHover()
        {
            if(m_highlightLayer > 0)
            {
                if(m_itemObject.layer != m_highlightLayer)
                {
                    m_itemObject.layer = m_highlightLayer;
                }
            }
        }

        public void ResetHightlight()
        {
            if (m_defaultLayer > 0)
            {
                if (m_itemObject.layer != m_defaultLayer)
                {
                    m_itemObject.layer = m_defaultLayer;
                }
            }
        }
    }
}
