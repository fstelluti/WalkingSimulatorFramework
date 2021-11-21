
namespace WalkingSimFramework.Helpers
{
    /// <summary>
    /// Interface to handle item proximity highlighting
    /// </summary>
    public interface IProximityHighlight
    {
        void CheckForHighlightableItemsInRange();
        void ClearCache();
    }
}
