using UnityEngine;

public class Repel : MonoBehaviour
{
    public bool willRepel = false;

    [SerializeField] float repelDistance = 1f;

    Vector3 mousePosition;

    RaycastHit2D raycastHit2D;

    Transform clickObject;

    private InputHandler at;

    [SerializeField] float speed = 5f;
    void Awake()
    {
        at = GetComponent<InputHandler>();
    }
    void Update()
    {

        mousePosition = Input.mousePosition;

        Ray mouseRay = Camera.main.ScreenPointToRay(mousePosition);

        if (Input.GetMouseButtonDown(1))
        {
            raycastHit2D = Physics2D.Raycast(mouseRay.origin, mouseRay.direction);
            clickObject = raycastHit2D ? raycastHit2D.collider.transform : null;

            if (clickObject.CompareTag("Magnet") && Mathf.Abs(transform.position.x - clickObject.transform.position.x) <= repelDistance && Mathf.Abs(transform.position.y - clickObject.transform.position.y) <= repelDistance)
            {
                willRepel = !willRepel;
                at.willAttract = false;
            }

        }

        if (willRepel && clickObject != null && clickObject.CompareTag("Magnet"))
        {
            Vector2 direction = ((transform.position - clickObject.transform.position)*-1).normalized;
            if (Mathf.Abs(transform.position.x - clickObject.transform.position.x) <= repelDistance && Mathf.Abs(transform.position.y - clickObject.transform.position.y) <= repelDistance)
            {
                Rigidbody2D rb = clickObject.GetComponent<Rigidbody2D>();
                rb.linearVelocity = direction * speed;

            }
            else
            {
                if (willRepel)
                {
                    willRepel = false;
                }
            }
        }

    }
}
