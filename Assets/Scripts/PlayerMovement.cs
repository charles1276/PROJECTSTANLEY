using UnityEngine;

using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    public float movementSpeed = 5f;

    public float jumpHeight = 10f;

    private float moveInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
        if(ctx.ReadValue<float>() == 1)
        {
            rb.linearVelocityY = jumpHeight;
        }
    }
}
