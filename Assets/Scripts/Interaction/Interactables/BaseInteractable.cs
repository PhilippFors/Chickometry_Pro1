using UnityEngine;

namespace Interaction.Interactables
{
    public enum InteractionPattern
    {
        RightClick,
        LeftClick,
        Interact,
        PickUp
    }

    /// <summary>
    /// Base class for items that can be interacted with (not picked up).
    /// </summary>
    public abstract class BaseInteractable : MonoBehaviour, IInteractable
    {
        public InteractionPattern pattern;
        public bool isInteractable;

        public virtual void OnInteract()
        {
        }
    }
}