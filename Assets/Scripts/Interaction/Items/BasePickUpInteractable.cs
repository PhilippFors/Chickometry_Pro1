using Interaction.Interactables;
using UnityEngine;

namespace Interaction.Items
{
    /// <summary>
    /// Base class for all items that the player can pick up.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class BasePickUpInteractable : BaseInteractable, IPickUpInteractable
    {
        public bool IsPickedUp => isPickedUp;
        protected bool isPickedUp;

        public override void OnInteract()
        {
            Debug.Log($"You just interacted with {name}");
        }

        public virtual void OnPickup()
        {
            isPickedUp = true;
            var col = GetComponentsInChildren<Collider>(true);
            foreach (var c in col) {
                c.isTrigger = true; // it just works
            }
        }

        public virtual void OnThrow()
        {
            isPickedUp = false;
            var col = GetComponentsInChildren<Collider>(true);
            foreach (var c in col) {
                c.isTrigger = false;
            }
        }
    }
}