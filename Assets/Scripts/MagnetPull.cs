using UnityEngine;

public class MagnetPull : MonoBehaviour
{
    public float speed;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Magnet"))
        {
            Vector2 direction = (transform.position - collision.transform.position).normalized;
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            rb.linearVelocity = direction * speed;
        }
    }
}
