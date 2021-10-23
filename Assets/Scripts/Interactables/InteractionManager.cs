using Entities.Player.PlayerInput;
using Interactables.Items;
using UnityEngine;
using Utlities;

namespace Interactables
{
    public class InteractionManager : MonoBehaviour
    {
        [SerializeField] private float throwForce;
        [SerializeField] private float interactionDistance = 4f;
        [SerializeField] private LayerMask interactableMask;
        private IInteractable currentSelected;
        // private IInteractable heldInteractable;
        private bool UseTriggered => PlayerInputController.Instance.LeftMouseButton.Triggered;
        private bool UsePressed => PlayerInputController.Instance.LeftMouseButton.IsPressed;
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

            // if (IsPickUpTrigggered)
            // {
            //     if (currentSelected != null)
            //     {
            //         currentSelected.OnInteract();
            //         heldInteractable = currentSelected;
            //         currentSelected = null;
            //     }
            // }

            if (InteractTriggered)
            {
                if (currentSelected != null)
                {
                    currentSelected.OnUse();
                }
            }

            // if (IsThrowTriggered)
            // {
            //     if (heldInteractable != null)
            //     {
            //         var rb = heldInteractable.GetComponent<Rigidbody>();
            //         rb.AddForce(Camera.main.gameObject.transform.forward * throwForce, ForceMode.Impulse);
            //         heldInteractable.OnThrow();
            //         heldInteractable = null;
            //     }
            // }
        }

        private void FindInteractable()
        {
            if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out var hit, interactionDistance, interactableMask))
            {
                var interactable = hit.transform.GetComponent<IInteractable>();
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