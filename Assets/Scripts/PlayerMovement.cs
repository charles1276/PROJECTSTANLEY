using UnityEngine;

using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    public float movementSpeed = 5f;

    public float jumpHeight = 10f;

    private float moveInput;

    private CapsuleCollider2D coll;

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
        rb.linearVelocityX = moveInput;
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>().x * movementSpeed;
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if(ctx.ReadValue<float>() == 1 && isGrounded())
        {
            rb.linearVelocityY = jumpHeight;
        }
    }
    private bool isGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
