using NaughtyAttributes;
using SmartData.SmartEquippedPanelData;
using SmartData.SmartInt;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WalkingSimFramework.Scriptable_Objects
{
    /// <summary>
    /// Demo class to manage the player's HUD system.
    /// </summary>
    public class HUDContainerUI : MonoBehaviour
    {
        [Space]
        [BoxGroup("Health")] [SerializeField] IntReader healthChangeVar;
        [BoxGroup("Health")] [SerializeField] IntReader defaultHealth;
        [BoxGroup("Health")] [SerializeField] IntReader maxHealth;

        [Space]
        [BoxGroup("Equipment")] [SerializeField] EquippedPanelDataReader equippedItemVar;

        [Space]
        [BoxGroup("Equipment")] [SerializeField] Image equippedItemImage;
        [BoxGroup("Equipment")] [SerializeField] TextMeshProUGUI equippedItemDescription;
        [BoxGroup("Equipment")] [SerializeField] TextMeshProUGUI healthValue;

        int currentHealth;

        string defaultEquipDescription;
        string currentEquipDescription;
        Sprite defaultEquipSprite;

        private void Awake()
        {
            currentHealth = defaultHealth;
            healthValue.SetText(currentHealth.ToString());

            defaultEquipDescription = equippedItemDescription.text;
            currentEquipDescription = defaultEquipDescription;

            defaultEquipSprite = equippedItemImage.sprite;
        }

        public void UpdateHealthValue()
        {
            currentHealth += healthChangeVar.value;
            int updatedHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            healthValue.SetText(updatedHealth.ToString());
        }

        public void UpdateCurrentlyEquippedItem()
        {
            if(!currentEquipDescription.Equals(equippedItemVar.value.Description))
            {
                equippedItemImage.sprite = equippedItemVar.value.ItemSprite;

                string equipDesc = equippedItemVar.value.Description;
                currentEquipDescription = equipDesc;

                equippedItemDescription.SetText(equipDesc);
            }
            else
            {
                equippedItemImage.sprite = defaultEquipSprite;
                equippedItemDescription.SetText(defaultEquipDescription);

                currentEquipDescription = defaultEquipDescription;
            }
        }
    }
}
