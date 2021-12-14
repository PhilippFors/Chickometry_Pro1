using System;
using Cysharp.Threading.Tasks.Triggers;
using Interaction.Interactables;
using RoomLoop.Portal;
using UnityEngine;

namespace Interaction.Items
{
    /// <summary>
    /// Base class for all items that the player can pick up.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class BasePickUpInteractable : BaseInteractable, IPickUpInteractable, IPortalTraveller
    {
        public bool IsPickedUp => isPickedUp;
        protected bool isPickedUp;
        protected Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
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

        public Vector3 PreviousPortalOffset { get; set; }
        public virtual bool CanTravel => true;
        public virtual void Teleport(Transform inPortal, Transform outPortal, Vector3 pos, Quaternion rot, Vector3 velocity)
        {
            transform.position = pos;
            transform.rotation = rot;
            rb.velocity = velocity;
        }
    }
}