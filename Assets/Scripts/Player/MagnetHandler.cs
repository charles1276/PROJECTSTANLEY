using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.InputSystem;

public class MagnetHandler : MonoBehaviour
{
    [SerializeField] public float attractDistance = 1f;
    [SerializeField] float speed = 5f;

    // track clicked object
    Transform clickObject;

    // polarity
    private ObjectPolarity attractionPolarity;

    // reference to object properties
    private ObjectProperties properties;

    void Start()
    {
        properties = gameObject.GetComponent<ObjectProperties>();
    }

    // input action for attracting
    public void Attract(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            attractionPolarity = ObjectPolarity.Positive;
            print("pos");
        }
        if (ctx.canceled)
        {
            attractionPolarity = ObjectPolarity.Neutral;
            print("neu");
        }
    }

    // input action for repelling
    public void Repel(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            attractionPolarity = ObjectPolarity.Negative;
            print("neg");
        }
        if (ctx.canceled)
        {
            attractionPolarity = ObjectPolarity.Neutral;
            print("neu");
        }
    }

    void Update()
    {
        // if neutral, do nothing
        if (attractionPolarity == ObjectPolarity.Neutral)
        {
            return;
        }

        // grab mouse position and raycast
        Vector3 mousePosition = Input.mousePosition;
        Ray mouseRay = Camera.main.ScreenPointToRay(mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(mouseRay.origin, mouseRay.direction);
        clickObject = hit ? hit.collider.transform : null; // get clicked object

        // check distance from player to clicked object
        Vector2 dist = transform.position - clickObject.transform.position;

        // if distance is greater than attractDistance, do nothing
        if (dist.magnitude > attractDistance)
        {
            return;
        }

        // if clicked object is a loose magnet, apply force based on weight comparison
        if (clickObject.CompareTag("Magnet"))
        {
            // compare weights
            string objPlayerInteraction = ObjectProperties.CompareWeights(properties.weight, clickObject.GetComponent<ObjectProperties>().weight);


            switch (objPlayerInteraction)
            {
                // player and object are equal weight
                case "Equal":
                    movePlayer(0.5f);
                    moveObject(0.5f);
                    break;

                // player is heavier
                case "Greater":
                    moveObject();
                    break;

                // object is heavier
                case "Less":
                    movePlayer();
                    break;
            }
        }
    }

    // apply force to player
    private void movePlayer(float speedMult = 1)
    {
        applyForce(gameObject, (clickObject.position - transform.position).normalized, speedMult);
    }

    // apply force to clicked object
    private void moveObject(float speedMult = 1)
    {
        applyForce(clickObject.gameObject, (transform.position - clickObject.position).normalized, speedMult);
    }

    private void applyForce(GameObject obj, Vector2 direction, float speedMult)
    {
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * speed * (int)attractionPolarity;
    }
}
