using Unity.Jobs;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.InputSystem;

public class MagnetHandler : MonoBehaviour
{
    [SerializeField] private float attractDistance = 1f;
    [SerializeField] private float speed = 5f;

    // track clicked object
    private Vector3 mouseWorldPosition;
    private Transform clickObject;

    // polarity
    private ObjectPolarity attractionPolarity;
    private Vector2 attractionVector;
    private int attractionBehavior;

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
            assignClickedObject();
            print("pos");
        }
        if (ctx.canceled)
        {
            attractionPolarity = ObjectPolarity.Neutral;
            unassignClickedObject();
            print("neu");
        }
    }

    // input action for repelling
    public void Repel(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            attractionPolarity = ObjectPolarity.Negative;
            assignClickedObject();
            print("neg");
        }
        if (ctx.canceled)
        {
            attractionPolarity = ObjectPolarity.Neutral;
            unassignClickedObject();
            print("neu");
        }
    }

    private void assignClickedObject()
    {
        // grab mouse position and raycast
        Vector3 mousePosition = Input.mousePosition;
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Ray mouseRay = Camera.main.ScreenPointToRay(mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(mouseRay.origin, mouseRay.direction);
        clickObject = hit ? hit.collider.transform : null; // get clicked object
    }

    private void unassignClickedObject()
    {
        clickObject = null;
    }

    void Update()
    {
        // if neutral, do nothing
        if (attractionPolarity == ObjectPolarity.Neutral)
        {
            return;
        }

        //// grab mouse position and raycast
        //Vector3 mousePosition = Input.mousePosition;
        //Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        //Ray mouseRay = Camera.main.ScreenPointToRay(mousePosition);

        //RaycastHit2D hit = Physics2D.Raycast(mouseRay.origin, mouseRay.direction);
        //clickObject = hit ? hit.collider.transform : null; // get clicked object

        // if no object clicked, do nothing
        if (clickObject == null)
        {
            return;
        }

        // check distance from player to clicked object
        attractionVector = mouseWorldPosition - transform.position;

        // if distance is greater than attractDistance, do nothing
        if (attractionVector.magnitude > attractDistance)
        {
            unassignClickedObject();
            return;
        }

        // if clicked object is a loose magnet, apply force based on weight comparison
        if (clickObject.CompareTag("Magnet"))
        {
            ObjectProperties clickObjectProperties = clickObject.GetComponent<ObjectProperties>();

            // multiplied by -1 so that like polarities repel and opposite polarities attract
            attractionBehavior = -1 * (int)attractionPolarity * (int)clickObjectProperties.polarity;

            // compare weights
            string objPlayerInteraction = ObjectProperties.CompareWeights(properties.weight, clickObjectProperties.weight);

            switch (objPlayerInteraction)
            {
                // player and object are equal weight
                case "Equal":
                    movePlayer();
                    moveObject();
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
        Vector2 attractionForce = attractionVector.normalized * speed * speedMult * (int)attractionPolarity;
        applyForce(gameObject, attractionForce);
    }

    // apply force to clicked object
    private void moveObject(float speedMult = 1)
    {
        Vector2 attractionForce = -attractionVector.normalized * speed * speedMult * (int)attractionPolarity;
        applyForce(clickObject.gameObject, attractionForce);
    }

    private void applyForce(GameObject obj, Vector2 force)
    {
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();

        print(obj.name.ToString() + ": " + force.ToString());

        // multiplied by mass to make the "force" acceleration
        rb.AddForce(force * rb.mass);
    }
}
