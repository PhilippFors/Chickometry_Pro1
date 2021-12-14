using Interaction.Interactables;
using RoomLoop.Portal;
using Sirenix.Utilities;
using UnityEngine;
using Utlities;

namespace Interaction.Items
{
    /// <summary>
    /// Base class for all items that the player can pick up.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class BasePickUpInteractable : BaseInteractable, IPickUpInteractable, IPortalTraveller
    {
        public Vector3 PreviousPortalOffset { get; set; }
        public virtual bool CanTravel => canTeleport;
        public bool IsPickedUp => isPickedUp;

        [SerializeField] protected bool canTeleport;
        
        protected bool isPickedUp;
        protected Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public override void OnInteract()
        {
        }

        public virtual void OnPickup()
        {
            isPickedUp = true;
            var col = GetComponentsInChildren<Collider>(true);
            foreach (var c in col) {
                c.isTrigger = true; // it just works
            }
            canTeleport = false;
            var children = GetComponentsInChildren<MeshRenderer>();
            children.ForEach(x => x.gameObject.layer = LayerIds.InteractablesTop);
        }

        public virtual void OnThrow()
        {
            isPickedUp = false;
            var col = GetComponentsInChildren<Collider>(true);
            foreach (var c in col) {
                c.isTrigger = false;
            }
            canTeleport = true;
            var children = GetComponentsInChildren<MeshRenderer>();
            children.ForEach(x => x.gameObject.layer = LayerIds.Interactable);
        }
        
        public virtual void Teleport(Transform inPortal, Transform outPortal, Vector3 pos, Quaternion rot, Vector3 velocity)
        {
            transform.position = pos;
            transform.rotation = rot;
            rb.velocity = velocity;
        }
    }
}