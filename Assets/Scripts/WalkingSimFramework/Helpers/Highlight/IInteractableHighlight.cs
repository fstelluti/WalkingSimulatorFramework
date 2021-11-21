using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WalkingSimFramework.Helpers.Highlight
{
    /// <summary>
    /// Used to determine how an item should be highlighted.
    /// </summary>
    public interface IInteractableHighlight
    {
        void InitInstance(GameObject _gameObject);

        void OnItemHover();

        void ResetHightlight();

    }
}
