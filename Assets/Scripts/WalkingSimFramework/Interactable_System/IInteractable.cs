
namespace WalkingSimFramework.Interactable_System
{
    /// <summary>
    /// Interface for interactable items
    /// </summary>
    public interface IInteractable
    {
        bool IsInteractable { get; }
        void OnInteract();
        void OnHoverOver();
    }
}
