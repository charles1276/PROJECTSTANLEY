using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    Rigidbody2D rb;

    bool hasTarget;

    public float attractSpeed = 5f;

    Vector3 targetPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        if(hasTarget)
        {
            Vector2 targetDirection = (targetPosition - transform.position).normalized;
            rb.linearVelocity = new Vector2(targetDirection.x, targetDirection.y) * attractSpeed;
        }
    }

    public void SetTarget(Vector3 position)
    {
        targetPosition = position;
        hasTarget = true;
    }
}
