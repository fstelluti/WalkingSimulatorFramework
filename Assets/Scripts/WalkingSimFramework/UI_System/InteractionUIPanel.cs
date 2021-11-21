using WalkingSimFramework.Helpers;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WalkingSimFramework.UI_System
{
    /// <summary>
    /// Class to handle the crosshair related UI
    /// </summary>
    public class InteractionUIPanel : MonoBehaviour
    {
        [Space, Header("Crosshair Settings")]
        [Required] [SerializeField] private GameObject defaultCrosshair;
        [Tooltip("Text to display under the crosshair. Not required")]
        [SerializeField] private TextMeshProUGUI crosshairTextUI;

        Image m_currentDefaultImage;
        RectTransform m_currentRectTransform;

        private void Awake()
        {
            m_currentDefaultImage = defaultCrosshair.GetComponent<Image>();
            m_currentRectTransform = defaultCrosshair.GetComponent<RectTransform>();

            // Construct default hover data
            InteractableItemCrosshairHoverInfo _defaultHoverData = new InteractableItemCrosshairHoverInfo();
            _defaultHoverData.CrosshairHoverSprite = m_currentDefaultImage.sprite;
            _defaultHoverData.CrosshairHoverImageSize = m_currentRectTransform.sizeDelta;
            _defaultHoverData.CrosshairHoverImageMaterial = m_currentDefaultImage.material;
            _defaultHoverData.CrosshairHoverText = string.Empty;

            // IMPORTANT - This should only be defined here
            InteractableItemCrosshairHoverInfo.defaultHoverInfo = _defaultHoverData;
        }

        public void UpdateCrosshair(InteractableItemCrosshairHoverInfo _crosshairHoverData)
        {
            m_currentDefaultImage.sprite = _crosshairHoverData.CrosshairHoverSprite;
            m_currentDefaultImage.material = _crosshairHoverData.CrosshairHoverImageMaterial;

            m_currentRectTransform.sizeDelta = _crosshairHoverData.CrosshairHoverImageSize;

            if(crosshairTextUI != null)
            {
                crosshairTextUI.SetText(_crosshairHoverData.CrosshairHoverText);
            }
        }
    }
}
