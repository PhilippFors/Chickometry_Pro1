using Entities.Player.PlayerInput;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpStrength;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundDistance;
    [SerializeField] private bool isGrounded;

    private Vector2 moveDirection => PlayerInputController.Instance.Movement.ReadValue();
    private bool jumpTriggered => PlayerInputController.Instance.Jump.Triggered;
    
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Jump();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded)
        {
            rb.drag = 2;
        }
        else
        {
            rb.drag = 0;
        }

        var moveDir = transform.rotation * new Vector3(moveDirection.x, 0, moveDirection.y) * moveSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + moveDir);
    }

    private void Jump()
    {
        if (isGrounded && jumpTriggered)
        {
            rb.AddForce(jumpStrength * Vector3.up, ForceMode.VelocityChange);
        }
    }
}