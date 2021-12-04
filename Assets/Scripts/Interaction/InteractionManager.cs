using Entities.Player.PlayerInput;
using Interaction.Interactables;
using Interaction.Items;
using UnityEngine;

namespace Interactables
{
    /// <summary>
    /// Handles interactions with BaseInteractables and BasePickupInteractables.
    /// </summary>
    public class InteractionManager : MonoBehaviour
    {
        [SerializeField] private float throwForce = 5f;
        [SerializeField] private float interactionDistance = 2f;
        [SerializeField] private LayerMask interactableMask;
        [SerializeField] private Transform itemParent;

        private BaseInteractable currentSelected;
        private BasePickUpInteractable currentlyHeldItem;
        private RigidbodyConstraints constraintCache; // needed for pickup items
        private bool ThrowTriggered => InputController.Instance.Triggered(InputPatterns.Throw);
        private bool InteractTriggered => InputController.Instance.Triggered(InputPatterns.Interact);
        private bool RightClickTriggered => InputController.Instance.Triggered(InputPatterns.RightClick);
        private bool RightClickIsPressed => InputController.Instance.IsPressed(InputPatterns.RightClick);

        private Camera mainCam;

        private void Awake()
        {
            mainCam = Camera.main;
        }

        private void Update()
        {
            FindInteractable();

            if (currentSelected) {
                if (InteractTriggered) {
                    if (currentSelected.pattern == InteractionPattern.PickUp) {
                        if (!currentlyHeldItem) {
                            var pickup = (BasePickUpInteractable) currentSelected;

                            currentlyHeldItem = pickup;
                            currentlyHeldItem.transform.rotation = itemParent.rotation;
                            currentlyHeldItem.transform.parent = itemParent;
                            currentlyHeldItem.transform.localPosition = Vector3.zero;
                            var rb = currentlyHeldItem.GetComponent<Rigidbody>();
                            rb.useGravity = false;
                            constraintCache = rb.constraints;
                            rb.constraints = RigidbodyConstraints.FreezeAll;
                            pickup.OnPickup();
                        }
                    }
                    
                    if (currentSelected.pattern == InteractionPattern.Interact) {
                        currentSelected.OnInteract();
                    }
                }

                if (RightClickTriggered || RightClickIsPressed) {
                    if (currentSelected.pattern == InteractionPattern.RightClick) {
                        currentSelected.OnInteract();
                    }
                }
            }

            if (currentlyHeldItem != null && ThrowTriggered) {
                currentlyHeldItem.transform.parent = null;
                currentlyHeldItem.transform.rotation = Quaternion.Euler(0, 0, 0);
                var rb = currentlyHeldItem.GetComponent<Rigidbody>();
                rb.useGravity = true;
                rb.constraints = RigidbodyConstraints.None;
                rb.constraints = constraintCache;
                rb.AddForce(Camera.main.gameObject.transform.forward * throwForce, ForceMode.Impulse);
                currentlyHeldItem.OnThrow();
                currentlyHeldItem = null;
            }
        }

        private void FindInteractable()
        {
            if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out var hit,
                interactionDistance,
                interactableMask, QueryTriggerInteraction.Ignore)) {
                var interactable = hit.transform.GetComponent<BaseInteractable>();

                if (interactable) {
                    currentSelected = interactable;
                }
            }
            else {
                currentSelected = null;
            }
        }
    }
}