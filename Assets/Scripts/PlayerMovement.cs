using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Movement")]
    public float movementSpeed = 5f;
    public float sprintMultiplier = 1.6f;

    private bool canWallJump;

    public float jumpHeight = 10f;
    private float jumpRequestedTime = -1f;
    private float jumpBufferTime = 0.2f; // basically how long before landing a jump input is still valid
    [SerializeField] private float coyoteTime = 0.2f;

    // input variables
    private float moveInput;
    private bool isSprinting;


    private CapsuleCollider2D coll;

    [Header("Statistics")]
    public float stamina = 100f; // drains when sprinting
    public float power = 100f;   // drains over time
    public float powerDrainRate = 1f;

    [Header("Mask for Ground Detection")]
    [SerializeField] private LayerMask jumpableGround;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // handle horizontal movement
        rb.linearVelocityX = moveInput;

        if (isSprinting)
        {
            rb.linearVelocityX *= sprintMultiplier;
        }

        // manage coyote time
        updateCoyoteTime();

        // handle jump
        attemptJump();
    }

    // FixedUpdate is called at a fixed interval and is independent of frame rate
    private void FixedUpdate()
    {
        if (isSprinting)
        {
            stamina -= 10f;

            // min stamina cap
            if (stamina <= 0f)
            {
                stamina = 0f;
                isSprinting = false;
            }
        }
        else
        {
            stamina += 5f;

            // max stamina cap
            if (stamina >= 100f)
            {
                stamina = 100f;
            }
        }
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>().x * movementSpeed;
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if(ctx.ReadValue<float>() == 1)
        {
            jumpRequestedTime = Time.time;
        }
    }

    public void Sprint(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && stamina > 0f)
        {
            isSprinting = true;
            print("ugh");
        }
        if (ctx.canceled)
        {
            isSprinting = false;
            print("ahh");
        }
    }

    private bool isGrounded()
    {
        // perform raycast downwards to check for ground
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + Vector2.down, Vector2.down, .01f, jumpableGround);

        // debugging ray :3c
        Debug.DrawRay((Vector2)transform.position + Vector2.down, Vector2.down * .01f, Color.red, 0.1f);
        return hit.collider != null;

        // boxcast method
        // slightly smaller than collider to avoid edge cases
        //return Physics2D.BoxCast(coll.bounds.center, Vector3.Scale(coll.bounds.size, new Vector3(0.9f, 1f, 1f)), 0f, Vector2.down, .1f, jumpableGround);
    }

    // update coyote time based on whether the player is grounded
    private void updateCoyoteTime()
    {
        if (isGrounded())
        {
            coyoteTime = 0.2f;
        }
        else
        {
            coyoteTime -= Time.deltaTime;
        }
    }

    // attempt to jump
    // jump if conditions are met
    private void attemptJump()
    {
        if (coyoteTime > 0f && jumpRequestedTime + jumpBufferTime > Time.time)
        {
            rb.linearVelocityY = jumpHeight;
        }
    }
}
