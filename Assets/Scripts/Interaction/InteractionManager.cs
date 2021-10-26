using Entities.Player.PlayerInput;
using Interaction.Interactables;
using Interaction.Items;
using UnityEngine;

namespace Interactables
{
    public class InteractionManager : MonoBehaviour
    {
        [SerializeField] private float throwForce = 3f;
        [SerializeField] private float interactionDistance = 4f;
        [SerializeField] private LayerMask interactableMask;
        [SerializeField] private Transform itemSlot;
        private BaseInteractable currentSelected;
        private BasePickUpInteractable currentlyHeldItem;
        private bool ThrowTriggered => PlayerInputController.Instance.ThrowItem.Triggered;
        private bool InteractTriggered => PlayerInputController.Instance.Interact.Triggered;

        private Camera mainCam;

        private void Awake()
        {
            mainCam = Camera.main;
        }

        private void Update()
        {
            FindInteractable();

            if (InteractTriggered && currentSelected)
            {
                if (!currentlyHeldItem && currentSelected is BasePickUpInteractable) {
                    var pickup = (BasePickUpInteractable) currentSelected;
                    currentlyHeldItem = pickup;
                    currentlyHeldItem.transform.rotation = itemSlot.rotation;
                    currentlyHeldItem.transform.parent = itemSlot;
                    currentlyHeldItem.transform.localPosition = Vector3.zero;
                    var rb = currentlyHeldItem.GetComponent<Rigidbody>();
                    rb.useGravity = false;

                    if (currentlyHeldItem.name.Contains("Gudrun")) {
                        rb.isKinematic = true;
                    }

                    pickup.OnPickup();
                }
                else {
                    currentSelected.OnInteract();
                }
            }

            if (ThrowTriggered)
            {
                if (currentlyHeldItem != null) {
                    currentlyHeldItem.transform.parent = null;
                    currentlyHeldItem.transform.rotation = Quaternion.Euler(0,0,0);
                    var rb = currentlyHeldItem.GetComponent<Rigidbody>();

                    if (currentlyHeldItem.name.Contains("Gudrun")) {
                        rb.isKinematic = false;
                    }

                    rb.useGravity = true;
                    rb.AddForce(Camera.main.gameObject.transform.forward * throwForce, ForceMode.Impulse);
                    currentlyHeldItem.OnThrow();
                    currentlyHeldItem = null;
                }
            }
        }

        private void FindInteractable()
        {
            if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out var hit, interactionDistance, interactableMask, QueryTriggerInteraction.Ignore))
            {
                var interactable = hit.transform.GetComponent<BaseInteractable>();

                if (interactable != null)
                {
                    currentSelected = interactable;
                }
            }
            else
            {
                currentSelected = null;
            }
        }
    }
}