using System;
using System.Collections;
using Checkpoints;
using DG.Tweening;
using ECM2.Characters;
using Entities.Player.PlayerInput;
using Interaction.Interactables;
using Interaction.Items;
using UI;
using UnityEngine;

namespace Interactables
{
    /// <summary>
    /// Handles interactions with interactables.
    /// </summary>
    public partial class InteractionManager : MonoBehaviour, IResettableBehaviour
    {
        [SerializeField] private float maxThrowForce = 10f;
        [SerializeField] private float minThrowForce = 2f;
        [SerializeField] private float interactionDistance = 2f;
        [SerializeField] private float placeDownDistance = 4f;
        [SerializeField] private float placeYOffset = 0.5f;
        [SerializeField] private LayerMask interactableMask;
        [SerializeField] private LayerMask placeDownMask;
        [SerializeField] private Transform itemParent;

        private MouseBasedRotator itemRotator;
        private FirstPersonCharacter playerMovement;
        private BaseInteractable currentSelected;
        private BasePickUpInteractable currentlyHeldItem;
        private RigidbodyConstraints constraintCache; // needed for pickup items
        
        private bool ThrowPressed => InputController.Instance.IsPressed(InputPatterns.Throw);
        private bool InteractTriggered => InputController.Instance.Triggered(InputPatterns.Interact);
        private bool RightClickTriggered => InputController.Instance.Triggered(InputPatterns.RightClick);
        private bool RightClickIsPressed => InputController.Instance.IsPressed(InputPatterns.RightClick);
        
        private Camera mainCam;
        private float pressTime;
        private float maxPressTime = 2f;
        private float throwTime;
        private float throwForce;
        private Quaternion ogItemRotation;
        private float oldItemMass;

        private void Awake()
        {
            playerMovement = GetComponent<FirstPersonCharacter>();
            
            mainCam = Camera.main;
            throwForce = minThrowForce;
            itemRotator = itemParent.GetComponent<MouseBasedRotator>();
            ogItemRotation = itemParent.localRotation;
        }

        private void Start()
        {
            InputController.Instance.Canceled(InputPatterns.Throw, ThrowRelease);
        }

        private void OnEnable()
        {
            CheckpointManager.Instance.RegisterBehaviour(this);
        }

        private void OnDisable()
        {
            CheckpointManager.Instance.UnregisterBehaviour(this);
        }

        private void Update()
        {
            FindInteractable();

            if (currentSelected) {
                HandelPickup();
                HandleInteract();
            }

            if (currentlyHeldItem) {
                MonitorMass();
                HandleUse();
            }

            HandleRightClick();
            HandleThrow();
        }

        private void HandleUse()
        {
            if (InteractTriggered) {
                currentlyHeldItem.OnUse(this);
                if (currentSelected) {
                    currentlyHeldItem.OnUseWithInteractable(currentSelected, this);
                }
            }
        }

        private void HandelPickup()
        {
            if (!currentlyHeldItem && InteractTriggered && currentSelected.pattern == InteractionPattern.PickUp) {
                var pickup = (BasePickUpInteractable) currentSelected;
                if (!pickup.canBePickedUp) {
                    return;
                }

                currentlyHeldItem = pickup;
                currentlyHeldItem.transform.rotation = itemParent.rotation;
                currentlyHeldItem.transform.parent = itemParent;
                currentlyHeldItem.transform.localPosition = Vector3.zero;
                var rb = currentlyHeldItem.GetComponent<Rigidbody>();
                rb.useGravity = false;
                var ignore = currentlyHeldItem.GetComponentInChildren<IgnoreSetKinematic>();
                if (!ignore) {
                    rb.isKinematic = true;
                }

                constraintCache = rb.constraints;
                rb.constraints = RigidbodyConstraints.FreezeAll;
                pickup.OnPickup();
                var playerRb = GetComponent<Rigidbody>();
                playerRb.mass += rb.mass;
                oldItemMass = rb.mass;
                playerMovement.SetJumpMultiplier(2);
            }
        }

        private void HandleInteract()
        {
            if (InteractTriggered && currentSelected.pattern == InteractionPattern.Interact) {
                currentSelected.OnInteract();
            }
        }

        private void HandleThrow()
        {
            if (currentlyHeldItem && ThrowPressed) {
                pressTime += Time.deltaTime;
            }

            if (pressTime > 0.5f) {
                if (throwTime < maxPressTime) {
                    throwTime += Time.deltaTime;
                    ThrowUIController.Instance.ShowSlider();
                    ThrowUIController.Instance.SetSliderValue(throwTime, maxPressTime);
                    throwForce = Mathf.Lerp(minThrowForce, maxThrowForce, (throwTime / maxPressTime) * 1.5f);
                }
            }
        }

        private void ThrowRelease()
        {
            if (currentlyHeldItem != null) {
                if (pressTime < 0.3f) {
                    if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out var hit,
                        placeDownDistance, placeDownMask, QueryTriggerInteraction.Ignore)) {
                        var dist = hit.distance;
                        currentlyHeldItem.transform.parent = null;
                        currentlyHeldItem.OnThrow();
                        StartCoroutine(PlaceDown(dist));
                    }
                    else {
                        var rb = currentlyHeldItem.GetComponent<Rigidbody>();
                        ReleaseRigidBody(rb);
                        rb.AddForce(Camera.main.gameObject.transform.forward * throwForce, ForceMode.Impulse);
                        ReleaseObject(false);
                    }
                }
                else {
                    var rb = currentlyHeldItem.GetComponent<Rigidbody>();
                    ReleaseRigidBody(rb);
                    rb.AddForce(Camera.main.gameObject.transform.forward * throwForce, ForceMode.Impulse);
                    ReleaseObject(false);
                }
            }

            ThrowUIController.Instance.HideSlider();
            throwForce = minThrowForce;
            pressTime = 0;
            throwTime = 0;
            itemParent.localRotation = ogItemRotation;
        }

        public void ReleaseRigidBody(Rigidbody rb)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = constraintCache;
        }
        
        public void ReleaseObject(bool withRb)
        {
            currentlyHeldItem.transform.parent = null;
            var rb = currentlyHeldItem.GetComponent<Rigidbody>();
            var playerRb = GetComponent<Rigidbody>();
            playerRb.mass -= rb.mass;
            if (withRb) {
                ReleaseRigidBody(rb);
            }
            currentlyHeldItem.OnThrow();
            currentlyHeldItem = null;
            playerMovement.SetJumpMultiplier(1);
        }

        private IEnumerator PlaceDown(float dist)
        {
            currentlyHeldItem.transform.DOMove(mainCam.transform.position + new Vector3(0, placeYOffset, 0) + mainCam.transform.forward * dist, 0.5f);
            yield return new WaitForSeconds(0.5f);
            var rb = currentlyHeldItem.GetComponent<Rigidbody>();
            var playerRb = GetComponent<Rigidbody>();
            playerRb.mass -= rb.mass;
            ReleaseRigidBody(rb);
            
            currentlyHeldItem = null;
            playerMovement.SetJumpMultiplier(1);
        }

        private void HandleRightClick()
        {
            if (currentSelected && (RightClickTriggered || RightClickIsPressed) &&
                currentSelected.pattern == InteractionPattern.RightClick) {
                currentSelected.OnInteract();
                return;
            }

            if (currentlyHeldItem && RightClickIsPressed) {
                itemRotator.OnInteract();
            }
        }

        private void FindInteractable()
        {
            if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out var hit,
                interactionDistance,
                interactableMask,
                QueryTriggerInteraction.Collide)) {
                var interactable = hit.transform.GetComponent<BaseInteractable>();

                currentSelected = interactable;
            }
            else {
                currentSelected = null;
            }
        }

        private void MonitorMass()
        {
            var rb = currentlyHeldItem.GetComponent<Rigidbody>();
            if (oldItemMass > rb.mass || oldItemMass < rb.mass) {
                var playerRb = GetComponent<Rigidbody>();
                playerRb.mass -= oldItemMass;
                playerRb.mass += rb.mass;
                oldItemMass = rb.mass;
            }
        }
    }
}