using UnityEngine;


public class InputHandler : MonoBehaviour
{
    bool willAttract = false;

    Vector3 mousePosition;

    RaycastHit2D raycastHit2D;

    Transform clickObject;

    public float speed = 5f;
    void Start()
    {

    }
    void Update()
    {
        mousePosition = Input.mousePosition;

        Ray mouseRay = Camera.main.ScreenPointToRay(mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            raycastHit2D = Physics2D.Raycast(mouseRay.origin, mouseRay.direction);
            clickObject = raycastHit2D ? raycastHit2D.collider.transform : null;

            if (clickObject.CompareTag("Magnet"))
            {
                willAttract = !willAttract;
            }
        }
        if (willAttract && clickObject != null && clickObject.CompareTag("Magnet"))
        {
            Vector2 direction = (transform.position - clickObject.transform.position).normalized;
            Rigidbody2D rb = clickObject.GetComponent<Rigidbody2D>();
            rb.linearVelocity = direction * speed;
        }
    }
}
