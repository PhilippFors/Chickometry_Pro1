using System;
using System.Collections;
using Checkpoints;
using Interaction.Interactables;
using Sirenix.Utilities;
using UnityEngine;
using Utlities;

namespace Interaction.Items
{
    /// <summary>
    /// Base class for all items that the player can pick up.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class BasePickUpInteractable : BaseInteractable, IPickUpInteractable, IResettableItem
    {
        public Vector3 OriginalPosition { get; set; }
        public Quaternion OriginalRotation { get; set; }
        public bool IsPickedUp => isPickedUp;
        
        protected bool isPickedUp;
        protected Rigidbody rb;

        private void Awake()
        {
            OriginalPosition = transform.position;
            OriginalRotation = transform.rotation;
            rb = GetComponent<Rigidbody>();
        }

        public virtual void OnPickup()
        {
            isPickedUp = true;
            var col = GetComponentsInChildren<Collider>(true);
            foreach (var c in col) {
                c.isTrigger = true; // it just works
            }
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
            var children = GetComponentsInChildren<MeshRenderer>();
            children.ForEach(x => x.gameObject.layer = LayerIds.Interactable);
        }

        public virtual void ResetToCheckpoint()
        {
            transform.position = OriginalPosition;
            transform.rotation = OriginalRotation;
        }
    }
}