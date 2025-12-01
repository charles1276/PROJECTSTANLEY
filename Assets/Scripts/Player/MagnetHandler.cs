using Unity.Jobs;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.InputSystem;

public class MagnetHandler : MonoBehaviour
{
    [Tooltip("Angle range (in degrees) within which the magnet will attract/repel objects.")]
    [Range(0f, 90f)]
    [SerializeField] private float attractionAngleRange = 10f;
    [SerializeField] private float attractionRange = 1f;
    [SerializeField] private float speed = 5f;

    // track clicked object
    private Vector3 mouseWorldPosition;
    private Transform clickObject;
    private Vector2 clickPoint;
    private int magnetsLayer;

    // polarity
    private ObjectPolarity attractionPolarity;
    private Vector2 attractionVector;
    private int attractionBehavior;

    // reference to object properties
    private ObjectProperties properties;

    void Start()
    {
        magnetsLayer = LayerMask.GetMask("Magnets", "AnchoredMagnets");
        properties = gameObject.GetComponent<ObjectProperties>();

        // reassign attraction angle range to cosine value for easier comparison later
        attractionAngleRange = Mathf.Cos(attractionAngleRange);
    }

    // input action for attracting
    public void Attract(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            attractionPolarity = ObjectPolarity.Positive;
            AssignClickedObject();
            //print("pos");
        }
        if (ctx.canceled && attractionPolarity == ObjectPolarity.Positive)
        {
            attractionPolarity = ObjectPolarity.Neutral;
            //print("neu");
        }
    }

    // input action for repelling
    public void Repel(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            attractionPolarity = ObjectPolarity.Negative;
            AssignClickedObject();
            //print("neg");
        }
        if (ctx.canceled && attractionPolarity == ObjectPolarity.Negative)
        {
            attractionPolarity = ObjectPolarity.Neutral;
            //print("neu");
        }
    }

    private void FindAttractionVector()
    {
        // testsdt
        Vector3 mousePosition = Input.mousePosition;
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // check distance from player to clicked object
        attractionVector = mouseWorldPosition - transform.position;
    }

    private void AssignClickedObject()
    {
        // grab mouse position and raycast
        Vector3 mousePosition = Input.mousePosition;
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Ray mouseRay = Camera.main.ScreenPointToRay(mousePosition);

        // i have no idea what the 1f does here but it works so
        RaycastHit2D hit = Physics2D.Raycast(mouseRay.origin, mouseRay.direction, 1f, magnetsLayer);
        clickObject = hit ? hit.collider.transform : null; // get clicked object

        FindAttractionVector();

        // if no object clicked, do nothing
        if (CheckGroundObstruction())
        {
            UnassignClickedObject();
        }
    }

    private void UnassignClickedObject()
    {
        clickObject = null;
    }

    private bool CheckGroundObstruction()
    {
        // raycast to check for walls
        RaycastHit2D magnetsCheck = Physics2D.Raycast(transform.position, attractionVector.normalized, attractionRange, LayerMask.GetMask("Magnets", "AnchoredMagnets", "Ground"));
        RaycastHit2D groundCheck = Physics2D.Raycast(transform.position, attractionVector.normalized, attractionRange, LayerMask.GetMask("Ground"));
        Debug.DrawRay(transform.position, attractionVector.normalized * attractionRange, Color.red);

        // if a wall is in the way, do nothing
        // does this by checking if both raycasts hit the same collider (meaning no magnet obstructed a wall)
        if (groundCheck.collider != null && groundCheck.collider == magnetsCheck.collider)
        {
            //print("Magnet influence blocked by walls.");
            return true;
        }

        // reassign clicked object if magnet collides 
        // only needed if multiple magnets can be in a line
        if (magnetsCheck.collider != null)
        {
            clickObject = magnetsCheck.collider.transform;
            clickPoint = magnetsCheck.point;
        }

        return false;
    }

    void Update()
    {
        // if neutral, do nothing
        if (attractionPolarity == ObjectPolarity.Neutral)
        {
            return;
        }

        // put magnetism display logic here later

        // if no object clicked, do nothing
        if (clickObject == null)
        {
            return;
        }

        FindAttractionVector();

        // if distance is greater than attractionRange, do nothing
        if ((clickObject.transform.position - transform.position).magnitude > attractionRange)
        {
            print("too far away, :3");
            return;
        }

        Vector3 objToPlayerDirection = (clickPoint - (Vector2)transform.position).normalized;

        // if object not within range
        if (Vector3.Dot(attractionVector.normalized, objToPlayerDirection) < attractionAngleRange)
        {
            //print("uhm,. u need to likeee,. actualy point at th objct.,, >.>");

            Debug.Log(Vector3.Dot(attractionVector.normalized, objToPlayerDirection));
            Debug.Log(attractionAngleRange);

            Debug.DrawRay(transform.position, attractionVector.normalized, Color.yellow);
            Debug.DrawRay(transform.position, objToPlayerDirection, Color.blue);

            return;
        }

        // if clicked object is a loose magnet, apply force based on weight comparison
        if (clickObject.CompareTag("Magnet"))
        {
            ObjectProperties clickObjectProperties = clickObject.GetComponent<ObjectProperties>();

            // multiplied by -1 so that like polarities repel and opposite polarities attract
            attractionBehavior = -1 * (int)attractionPolarity * (int)clickObjectProperties.polarity;
            //print(attractionPolarity);
            //print(clickObjectProperties.polarity);
            //print(attractionBehavior);

            // compare weights
            string objPlayerInteraction = ObjectProperties.CompareWeights(properties.weight, clickObjectProperties.weight);

            switch (objPlayerInteraction)
            {
                // player and object are equal weight
                case "Equal":
                    MovePlayer();
                    MoveObject();
                    break;

                // player is heavier
                case "Greater":
                    MoveObject();
                    break;

                // object is heavier
                case "Less":
                    MovePlayer();
                    break;
            }
        }
    }

    // apply force to player
    private void MovePlayer(float speedMult = 1)
    {
        Vector2 attractionForce =  speed * speedMult * attractionBehavior * attractionVector.normalized;
        ApplyForce(gameObject, attractionForce);
    }

    // apply force to clicked object
    private void MoveObject(float speedMult = 1)
    {
        Vector2 attractionForce = speed * speedMult * attractionBehavior * -attractionVector.normalized;
        ApplyForce(clickObject.gameObject, attractionForce);
    }

    private void ApplyForce(GameObject obj, Vector2 force)
    {
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();

        //print(obj.name.ToString() + ": " + force.ToString());

        // multiplied by mass to make the "force" acceleration
        rb.AddForce(force * rb.mass);
    }
}
