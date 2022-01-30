using Entities.Player.PlayerInput;
using RoomLoop.Portal;
using UnityEngine;

namespace Entities.Player
{
    public class PlayerMovement : MonoBehaviour, IPortalTraveller
    {
        public Vector3 PreviousPortalOffset { get; set; }
        public bool CanTravel => true;

        [SerializeField] private float moveSpeed;
        [SerializeField] private float jumpStrength;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private float groundCheckRadius;
        [SerializeField] private bool isGrounded;
        [SerializeField] private float notGroundedDelay;

        private Vector2 moveDirection => InputController.Instance.GetValue<Vector2>(InputPatterns.Movement);
        private bool jumpTriggered => InputController.Instance.Triggered(InputPatterns.Jump);
        private Rigidbody rb;
        private bool moveEnabled = true;
        private bool isJumping;
        private float notGroundedTimer;
        private int jumpFrameDelay;
        private float jumpdivider;
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            Jump();
            
            if (jumpFrameDelay > 2 || !isJumping) {
                if (Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask,
                    QueryTriggerInteraction.Ignore)) {
                    isGrounded = true;
                    notGroundedTimer = 0;
                    isJumping = false;
                }
                else {
                    if (isJumping) {
                        isGrounded = false;
                    }
                    else {
                        notGroundedTimer += Time.deltaTime;
                        if (notGroundedTimer >= notGroundedDelay) {
                            isGrounded = false;
                        }
                    }
                }

                jumpFrameDelay = 0;
            }
            
            if (isJumping) {
                jumpFrameDelay++;
            }
            
            if (isGrounded) {
                rb.drag = 1;
            }
            else {
                rb.drag = 0;
            }

            if (!moveEnabled) {
                return;
            }

            var moveDir = transform.rotation * new Vector3(moveDirection.x, 0, moveDirection.y) *
                          (moveSpeed * Time.deltaTime);

            transform.position += moveDir;
        }

        public void SetJumpDivder(float val)
        {
            jumpdivider = val;
        }
        
        private void Jump()
        {
            if (isGrounded && jumpTriggered && !isJumping) {
                rb.velocity += jumpStrength / jumpdivider * Vector3.up;
                isJumping = true;
                isGrounded = false;
            }
        }

        public void Teleport(Transform inPortal, Transform outPortal, Vector3 pos, Quaternion rot, Vector3 velocity)
        {
            moveEnabled = false;
            var mouselook = GetComponentInChildren<SmoothMouseLook>();
            mouselook.ForceLookAt(rot);
            transform.position = pos;
            Physics.SyncTransforms();
            moveEnabled = true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}