using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Movement")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 1.6f;
    [SerializeField] private float friction = 0.9f;
    private float originalFriction;

    private bool canWallJump;

    [SerializeField] private float jumpHeight = 10f;
    private float jumpRequestedTime = -1f;
    private float jumpBufferTime = 0.2f; // basically how long before landing a jump input is still valid
    [SerializeField] private float coyoteTime = 0.2f;

    //Gravity Swap
    private float gravityStore;



    // input variables
    private float moveInput;
    private bool isSprinting;

    private CapsuleCollider2D coll;

    [Header("Mask for Ground Detection")]
    [SerializeField] private LayerMask jumpableGround;

    private PlayerStats stats;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // store original friction
        originalFriction = friction;

        // get reference to player stats
        stats = GetComponent<PlayerStats>();

        // get reference to rigidbody and collider
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();

        // store gravity scale
        gravityStore = rb.gravityScale;

    }

    // Update is called once per frame
    void Update()
    {
        // handle horizontal movement
        float movementForce = moveInput;
        if (isSprinting)
        {
            movementForce *= sprintMultiplier;
        }

        rb.AddForce(new Vector2(movementForce, 0f) * rb.mass);
        rb.linearVelocityX *= friction; // friction

        // manage coyote time
        updateCoyoteTime();

        // handle jump
        attemptJump();

    }

    // FixedUpdate is called at a fixed interval and is independent of frame rate
    private void FixedUpdate()
    {
        attemptSprint();
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
        if (ctx.performed)
        {
            isSprinting = true;
        }
        if (ctx.canceled)
        {
            isSprinting = false;
        }
    }

    public void SwapGravity(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            gravityStore = -gravityStore;
        }
    }


    private bool isGrounded()
    {
        Vector2 worldDown = Mathf.Sign(gravityStore) * Vector2.down;

        // perform raycast downwards to check for ground
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + worldDown, worldDown, .01f, jumpableGround);

        // debugging ray :3c
        Debug.DrawRay((Vector2)transform.position + worldDown, worldDown * .01f, Color.red, 0.1f);
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
            rb.linearVelocityY = jumpHeight * gravityStore/2;

        }
        
    }

    private void attemptSprint()
    {
        // drain stamina while sprinting
        if (isSprinting && rb.linearVelocityX != 0)
        {
            // only sprint if the player has stamina
            if (!stats.stamina.CanUse()) { return; }

            stats.stamina.Drain();
        }
    }

    // ignore friction effect
    public void startIgnoreFriction()
    {
        rb.linearVelocityX /= friction;
    }

    // reset friction effect
    public void stopIgnoreFriction()
    {
        rb.linearVelocityX *= friction;
    }
}
