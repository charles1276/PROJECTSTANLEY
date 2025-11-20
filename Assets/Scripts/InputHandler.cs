using UnityEngine;


public class InputHandler : MonoBehaviour
{
    bool willAttract = false;

    [SerializeField] float attractDistance = 1f;

    Vector3 mousePosition;

    RaycastHit2D raycastHit2D;

    Transform clickObject;

    [SerializeField] float speed = 5f;
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

            if (clickObject.CompareTag("Magnet") && Mathf.Abs(transform.position.x - clickObject.transform.position.x) <= attractDistance && Mathf.Abs(transform.position.y - clickObject.transform.position.y) <= attractDistance)
            {
                willAttract = !willAttract;
            }
            
        }
        
        if (willAttract && clickObject != null && clickObject.CompareTag("Magnet"))
        
        {
            Vector2 direction = (transform.position - clickObject.transform.position).normalized;
            if(Mathf.Abs(transform.position.x - clickObject.transform.position.x) <= attractDistance && Mathf.Abs(transform.position.y - clickObject.transform.position.y) <= attractDistance)
            {
                Rigidbody2D rb = clickObject.GetComponent<Rigidbody2D>();
                rb.linearVelocity = direction * speed;

            }
        }
        
    }
}
