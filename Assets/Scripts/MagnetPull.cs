using UnityEngine;

public class MagnetPull : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Magnetic"))
        {
            Vector2 direction = (transform.position - collision.transform.position).normalized;
            float distance = Vector2.Distance(transform.position, collision.transform.position);
            float forceMagnitude = 10f / (distance * distance); // Inverse square law for force
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(direction * forceMagnitude);
            }
        }
    }
}
