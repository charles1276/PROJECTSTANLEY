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

    [SerializeField] private float jumpHeight = 10f;
    private float jumpRequestedTime = -1f;
    private float jumpBufferTime = 0.2f; // basically how long before landing a jump input is still valid
    [SerializeField] private float coyoteTime = 0.2f;
    private bool doubleJump = false;

    [SerializeField] private Animator animator; // THE ANIMATORRR BGVSC Mbvv,mb,....,,.

    // input variables
    private float moveInput;
    private bool isSprinting;
    private bool swappedGravity;

    [Header("Mask for Ground Detection")]
    [SerializeField] private LayerMask jumpableGround;

    private PlayerStats stats;
    private InventoryManager inventory;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // get reference to player stats
        stats = GetComponent<PlayerStats>();

        // get reference to inventory
        inventory = GetComponent<InventoryManager>();

        // get reference to rigidbody and collider
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // handle horizontal movement
        float movementForce = moveInput;
        if (attemptSprint())
        {
            movementForce *= sprintMultiplier;
        }

        rb.AddForce(new Vector2(movementForce, 0f) * rb.mass);
        rb.linearVelocityX *= friction; // friction

        // manage coyote time
        updateCoyoteTime();

        // handle jump
        attemptJump();

        // gravity power consumption (cuz im too lazy :sob)
        if (Mathf.Sign(rb.gravityScale) < 0)
        {
            stats.power.Drain();
        }
    }

    // FixedUpdate is called at a fixed interval and is independent of frame rate
    private void FixedUpdate()
    {
        attemptFlipGravity();

        // ANIMATOR
        animator.SetFloat("xVel", Mathf.Abs(moveInput));
        animator.SetBool("isSprinting", isSprinting);
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
        if (ctx.performed && inventory.HasItem("GravityBoots") && stats.power.CanUse())
        {
            FlipGravity();
        }
    }

    private void FlipGravity()
    {
        rb.gravityScale *= -1;
        coyoteTime = 0f; // reset coyote time on gravity swap

        swappedGravity = !swappedGravity;
    }

    private bool isGrounded()
    {
        Vector2 worldDown = Mathf.Sign(rb.gravityScale) * Vector2.down;

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
        if (coyoteTime > 0f && jumpRequestedTime + jumpBufferTime > Time.time || doubleJump)
        {
            rb.linearVelocityY = jumpHeight * Mathf.Sign(rb.gravityScale);
        }
    }

    private bool attemptSprint()
    {
        // player has to actually be sprinting
        if (!isSprinting)
        {
            return false;
        }

        // drain stamina while sprinting
        if (Mathf.Abs(rb.linearVelocityX) < 0.1f)
        {
            return false;
        }

        // only sprint if the player has stamina
        if (!stats.stamina.CanUse())
        {
            isSprinting = false;
            return false;
        }

        stats.stamina.Drain();
        return true;
    }

    private bool attemptFlipGravity()
    {
        // drain power when flipped
        if (!swappedGravity)
        {
            return false;
        }

        // when they run out of power
        if (!stats.power.CanUse())
        {
            FlipGravity();
            return false;
        }

        stats.power.Drain();
        return true;
    }
}
