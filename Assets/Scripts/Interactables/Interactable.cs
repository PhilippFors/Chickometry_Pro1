using UnityEngine;

namespace Interactables
{
    public abstract class Interactable : MonoBehaviour, IInteractable
    {
        public abstract void OnUse();
    }
}
