using Checkpoints;
using Interactables;
using Interaction.Interactables;
using Sirenix.Utilities;
using UnityEngine;
using Utilities;
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
        public bool canBePickedUp;

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
                if (!c.GetComponent<IgnoreColliderChange>()) {
                    c.isTrigger = true; // it just works
                }
            }

            ChangeLayer(true);
        }

        protected void ChangeLayer(bool pickUp)
        {
            var children = GetComponentsInChildren<MeshRenderer>();
            children.ForEach(x =>
            {
                var layerOv = x.GetComponent<LayerChangeOverride>();

                if (pickUp) {
                    if (layerOv && layerOv.overrideLayer != -1) {
                        x.gameObject.layer = layerOv.overrideLayer;
                    }
                    else if(!layerOv){
                        x.gameObject.layer = LayerIds.InteractablesTop;
                    }
                }
                else {
                    if (layerOv && layerOv.overrideLayer != -1) {
                        x.gameObject.layer = layerOv.defaultLayer;
                    }
                    else if(!layerOv) {
                        x.gameObject.layer = LayerIds.Interactable;
                    }
                }
            });
        }

        public virtual void OnThrow()
        {
            isPickedUp = false;
            var col = GetComponentsInChildren<Collider>(true);
            foreach (var c in col) {
                if (!c.GetComponent<IgnoreColliderChange>()) {
                    c.isTrigger = false; // it just works
                }
            }

            ChangeLayer(false);
        }

        public virtual void ResetToCheckpoint()
        {
            transform.position = OriginalPosition;
            transform.rotation = OriginalRotation;
        }

        public virtual void OnUseWithInteractable(BaseInteractable interactable, InteractionManager manager)
        {
        }

        public virtual void OnUse(InteractionManager manager)
        {
        }
    }
}