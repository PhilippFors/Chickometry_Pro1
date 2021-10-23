using Entities.Player.PlayerInput;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpStrength;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundDistance;
    
    private Rigidbody rb;
    private Vector2 moveDirection;
    [SerializeField] private bool isGrounded;
    private float stopTimer = 0.25f;
    private float stopTimerCount = 0;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PlayerInputController.Instance.Jump.Performed += ctx => Jump(ctx);
        PlayerInputController.Instance.Movement.Performed += ctx => MoveDirection(ctx);
        PlayerInputController.Instance.Movement.Canceled += ctx => MoveDirection(ctx);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        StopMovement();
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
        if (moveDirection != Vector2.zero)
            stopTimerCount = 0f;
        
        Vector3 temp = transform.rotation * new Vector3(moveDirection.x, 0, moveDirection.y);
        
        rb.AddForce(temp.normalized * moveSpeed / 0.75f, ForceMode.Acceleration);

        if (isGrounded)
        {
            Vector3 xzClamped = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            xzClamped = Vector3.ClampMagnitude(xzClamped, moveSpeed);
            rb.velocity = xzClamped + new Vector3(0, rb.velocity.y, 0);
        }
    }

    private void StopMovement()
    {
        if (moveDirection == Vector2.zero && isGrounded)
        {
            stopTimerCount += Time.deltaTime;

            if (stopTimerCount >= stopTimer)
            {
                rb.velocity = Vector3.zero;
            }
        }
    }

    private void MoveDirection(InputAction.CallbackContext ctx)
    {
        moveDirection = ctx.ReadValue<Vector2>();
    }

    private void Jump(InputAction.CallbackContext ctx)
    {
        if (isGrounded)
        {
            rb.AddForce(jumpStrength * Vector3.up, ForceMode.VelocityChange);
            stopTimerCount = 0f;
        }
    }
}