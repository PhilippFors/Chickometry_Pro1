using Interaction.Interactables;
using UnityEngine;
using Utlities;

namespace Interaction.Items
{
    /// <summary>
    /// Base class for all items that the player can pick up.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class BasePickUpInteractable : BaseInteractable, IPickUpInteractable
    {
        public bool IsPickedUp => isPickedUp;
        protected VelocityTracker velocityTracker;
        protected bool isPickedUp;
        private void OnEnable()
        {
            // velocityTracker = ServiceLocator.Get<VelocityTracker>();
            // if (!velocityTracker) {
            //     return;
            // }
            // velocityTracker.Register(this);
        }
        
        private void OnDisable()
        {
            // if (!velocityTracker) {
            //     return;
            // }
            // velocityTracker.Unregister(this);
        }

        public override void OnInteract()
        {
            Debug.Log($"You just interacted with {name}");
        }

        public virtual void OnPickup()
        {
            isPickedUp = true;
            var col = GetComponentsInChildren<Collider>(true);
            foreach (var c in col) {
                c.enabled = false;
            }
        }

        public virtual void OnThrow()
        {
            isPickedUp = false;
            var col = GetComponentsInChildren<Collider>(true);
            foreach (var c in col) {
                c.enabled = true;
            }
        }

        public virtual void OnCollisionEnter(Collision other)
        {
            if(velocityTracker) {
                var vel = velocityTracker.GetVelocity(this);
                if (vel > 2f) {
                    // Play sfx
                }

                if (vel > 20f) {
                    Debug.Log($"Just hit something with {vel} velocity");
                }
            }
        }
    }
}