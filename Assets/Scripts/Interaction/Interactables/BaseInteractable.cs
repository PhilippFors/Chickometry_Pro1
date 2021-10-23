using UnityEngine;

namespace Interaction.Interactables
{
    public abstract class BaseInteractable : MonoBehaviour, IInteractable
    {
        public abstract void OnUse();
    }
}
