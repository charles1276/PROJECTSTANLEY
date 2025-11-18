using UnityEngine;
using UnityEngine.InputSystem;

public class MagnetPull : MonoBehaviour
{
    public float speed;

    bool willAttract = false;


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Magnet") && willAttract == true)
        {
            Vector2 direction = (transform.parent.position - collision.transform.position).normalized;
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            rb.linearVelocity = direction * speed;
        }
    }
    public void Attract(InputAction.CallbackContext ctx)
    {
        willAttract = !willAttract;
    }
}
