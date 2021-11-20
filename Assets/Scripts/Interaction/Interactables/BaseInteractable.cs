using UnityEngine;

namespace Interaction.Interactables
{
    /// <summary>
    /// Base class for items that can be interacted with (not picked up).
    /// </summary>
    public abstract class BaseInteractable : MonoBehaviour, IInteractable
    {
        public abstract void OnInteract();
    }
}