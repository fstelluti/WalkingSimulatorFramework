using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WalkingSimFramework.Helpers
{
    /// <summary>
    /// Groups together data to manage the crosshair when it hovers over an interactable item
    /// </summary>
    [System.Serializable]
    public class InteractableItemCrosshairHoverInfo 
    {
        [SerializeField] private Sprite crosshairSprite;
        [SerializeField] private Material crosshairImageMaterial;
        [SerializeField] private Vector2 crosshairImageSize;

        [Space]
        #pragma warning disable 0414
        [SerializeField] private bool showText = true;
        #pragma warning restore 0414
        [EnableIf("showText")]
        [AllowNesting]
        [SerializeField] private string statusText;

        readonly static InteractableItemCrosshairHoverInfo m_emptyItemHoverInfo = new InteractableItemCrosshairHoverInfo();

        public InteractableItemCrosshairHoverInfo()
        {
            crosshairImageSize = Vector2.zero;
            statusText = string.Empty;
        }

        // Static values for specific data

        // Initial crosshair data
        public static InteractableItemCrosshairHoverInfo defaultHoverInfo { get; set; }

        // Used when want to 'clear' the crosshair
        public static InteractableItemCrosshairHoverInfo emptyHoverInfo => m_emptyItemHoverInfo;

        public void SetHoverInfo(InteractableItemCrosshairHoverInfo interactableItemCrosshairHoverInfo)
        {
            crosshairSprite = interactableItemCrosshairHoverInfo.crosshairSprite;
            crosshairImageMaterial = interactableItemCrosshairHoverInfo.crosshairImageMaterial;
            crosshairImageSize = interactableItemCrosshairHoverInfo.crosshairImageSize;
            statusText = interactableItemCrosshairHoverInfo.statusText;
        }

        public Sprite CrosshairHoverSprite
        {
            get { return crosshairSprite; } 
            set { crosshairSprite = value; }
        }
        public Material CrosshairHoverImageMaterial
        {
            get { return crosshairImageMaterial; }
            set { crosshairImageMaterial = value; }
        }

        public Vector2 CrosshairHoverImageSize
        {
            get { return crosshairImageSize; }
            set { crosshairImageSize = value; }
        }
        public string CrosshairHoverText
        {
            get { return showText ? statusText : string.Empty; }
            set { statusText = value; }
        }
    }
}