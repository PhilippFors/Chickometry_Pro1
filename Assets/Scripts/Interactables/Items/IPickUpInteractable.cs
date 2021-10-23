namespace Interactables.Items
{
    /// <summary>
    /// Interface for interactables that can be picke up by the player.
    /// </summary>
    public interface IPickUpInteractable : IInteractable
    {
        void OnInteract();
        void OnThrow();
    }
}
