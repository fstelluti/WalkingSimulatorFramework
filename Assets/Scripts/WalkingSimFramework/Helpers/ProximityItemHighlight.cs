using WalkingSimFramework.Interactable_System;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace WalkingSimFramework.Helpers
{
    /// <summary>
    /// Manages how items are highlighted based on their proximity to the player
    /// </summary>
    public class ProximityItemHighlight : IProximityHighlight
    {
        /// <summary>
        /// Map of highlighted objects with the corresponding default materials
        /// </summary>
        private readonly Dictionary<GameObject, InteractableBase> m_highlightedItemsCache; 
        private readonly Collider[] m_proximityHitColliders;

        private List<GameObject> m_selectableItems;
        private List<GameObject> m_removableItems;

        private readonly float m_proximityRangeRadius;

        private readonly Camera m_player_cam;

        /// <summary>
        /// Layers that should be highlighted
        /// </summary>
        private readonly LayerMask m_highlightableItemLayers;

        public ProximityItemHighlight(int _maxNumberHighlightedItems, float _proximityRangeRadius, Camera _playerCam, LayerMask _highlightableItemLayers)
        {
            m_highlightedItemsCache = new Dictionary<GameObject, InteractableBase>();
            m_proximityHitColliders = new Collider[_maxNumberHighlightedItems];

            m_proximityRangeRadius = _proximityRangeRadius;

            m_player_cam = _playerCam;
            m_highlightableItemLayers = _highlightableItemLayers;

            m_selectableItems = new List<GameObject>(_maxNumberHighlightedItems);
            m_removableItems = new List<GameObject>(_maxNumberHighlightedItems);
        }

        /// <summary>
        /// Highlights the items in range of the player
        /// </summary>
        public void CheckForHighlightableItemsInRange()
        {
            // Order matters here
            FindSelectableItems();
            RemoveHighlightedItems();  // Possibly, if any were lost
            HighlightItems();
        }

        private void FindSelectableItems()
        {
            int _foundColliders = Physics.OverlapSphereNonAlloc(m_player_cam.transform.position + m_player_cam.transform.forward * m_proximityRangeRadius, m_proximityRangeRadius, m_proximityHitColliders, m_highlightableItemLayers);

            m_selectableItems.Clear();

            for (int i = 0; i < _foundColliders; i++)
            {
                if (m_proximityHitColliders[i] != null)
                {
                    m_selectableItems.Add(m_proximityHitColliders[i].gameObject);
                }
            }
        }

        private void RemoveHighlightedItems()
        {
            if (m_selectableItems.Count < m_highlightedItemsCache.Count)
            {
                m_removableItems.Clear();

                // Reset materials that are not actually selected
                foreach (KeyValuePair<GameObject, InteractableBase> item in m_highlightedItemsCache)
                {
                    if (item.Key == null)
                    {
                        m_removableItems.Add(item.Key);  // Remove null values as well
                    }
                    else if (!m_selectableItems.Contains(item.Key))
                    {
                        item.Value.ResetAppearance();

                        m_removableItems.Add(item.Key);
                    }
                }

                // Remove items that are no longer highlighted
                foreach (GameObject removeItem in m_removableItems)
                {
                    m_highlightedItemsCache.Remove(removeItem);
                }
            }
        }

        private void HighlightItems()
        {
            foreach (GameObject item in m_selectableItems)
            {
                // Add to cache if not in the set
                if (!m_highlightedItemsCache.ContainsKey(item))
                {
                    // Can just modify this directly, instead of going through InteractionableData, beacuse don't need to change any states elsewhere
                    InteractableBase _interactable = item.transform.GetComponent<InteractableBase>();

                    if (_interactable != null && _interactable.IsInteractable)
                    {
                        m_highlightedItemsCache.Add(item, _interactable);

                        // Seems to always enter here
                        _interactable.OnHoverOver();
                    }
                }
            }
        }

        public void ClearCache()
        {
            m_highlightedItemsCache.Clear();
        }
    }
}