using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Vector2Extensions
{
    // converts angle (in degrees) in degrees to vector components
    public static Vector2 AngleToComponents(float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector2(cos, sin);
    }

    public static Vector2 Rotate(Vector2 vector, Vector2 angleComponents)
    {
        float newX = vector.x * angleComponents.x - vector.y * angleComponents.y;
        float newY = vector.x * angleComponents.y + vector.y * angleComponents.x;
        return new Vector2(newX, newY);
    }
}

public class MagnetHandler : MonoBehaviour
{
    [Tooltip("Angle range (in degrees) within which the magnet will attract/repel objects.")]
    [Range(0f, 90f)]
    [SerializeField] private float attractionAngleRange = 10f;
    [SerializeField] private float attractionRange = 1f;
    [Tooltip("How many divisions there when checking magnet attraction. May help performance if reduced.")]
    [Range(3, 25)]
    [SerializeField] private int attractionDivisions = 5;
    [SerializeField] private float speed = 5f;

    // track clicked object
    private Vector3 mouseWorldPosition;
    private GameObject attractedObject;
    private Vector2 attractedPoint;

    // polarity
    public ObjectPolarity attractionPolarity;

    // reference to object properties
    private ObjectProperties properties;
    private PlayerStats playerStats;

    // hud manager
    private HUDManager hudManager;

    void Start()
    {
        properties = gameObject.GetComponent<ObjectProperties>();
        playerStats = gameObject.GetComponent<PlayerStats>();

        hudManager = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDManager>();
    }

    // input action for attracting
    public void Attract(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            attractionPolarity = ObjectPolarity.Positive;
            //print("pos");
        }
        if (ctx.canceled && attractionPolarity == ObjectPolarity.Positive)
        {
            attractionPolarity = ObjectPolarity.Neutral;
            //print("neu");
        }

        // update HUD
        hudManager.updateMagnetismIndicator(attractionPolarity);
    }

    // input action for repelling
    public void Repel(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            attractionPolarity = ObjectPolarity.Negative;
            //print("neg");
        }
        if (ctx.canceled && attractionPolarity == ObjectPolarity.Negative)
        {
            attractionPolarity = ObjectPolarity.Neutral;
            //print("neu");
        }

        // update HUD
        hudManager.updateMagnetismIndicator(attractionPolarity);
    }

    private void UnassignClickedObject()
    {
        attractedObject = null;
    }

    private void CastAttractionCone()
    {
        // reset attracted object each frame
        UnassignClickedObject();

        // testsdt
        Vector3 mousePosition = Input.mousePosition;
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // check distance from player to clicked object
        Vector2 middleAttractionVector = mouseWorldPosition - transform.position;

        // raycast to check for walls
        RaycastHit2D[] middleChecks = FindGroundCheckVectors(middleAttractionVector);

        // check middle vector first
        UpdateAttractedObject(middleChecks);

        // already found an attracted object, so skip further checks
        if (attractedObject != null)
        {
            return;
        }

        // start at one extreme of the attraction angle range
        Vector2 AttractionVector = Vector2Extensions.Rotate(middleAttractionVector, Vector2Extensions.AngleToComponents(-attractionAngleRange));

        // iterate through divisions to find every magnet that could be attracted
        float angleDifference = attractionAngleRange * 2 / (attractionDivisions - 1);
        Vector2 angleDifferenceComponents = Vector2Extensions.AngleToComponents(angleDifference);

        for (int i = 0; i < attractionDivisions; i++)
        {
            // raycast to check for walls
            RaycastHit2D[] checks = FindGroundCheckVectors(AttractionVector);

            // turn vector for next iteration
            AttractionVector = Vector2Extensions.Rotate(AttractionVector, angleDifferenceComponents);

            // check raycast results
            UpdateAttractedObject(checks);
        }
    }

    private RaycastHit2D[] FindGroundCheckVectors(Vector2 vector)
    {
        // raycast to check for walls
        RaycastHit2D magnetsCheck = Physics2D.Raycast(transform.position, vector.normalized, attractionRange, LayerMask.GetMask("Magnets", "AnchoredMagnets", "Ground"));
        RaycastHit2D groundCheck = Physics2D.Raycast(transform.position, vector.normalized, attractionRange, LayerMask.GetMask("Ground"));
        Debug.DrawRay(transform.position, vector.normalized * attractionRange, Color.red);

        return new RaycastHit2D[] { magnetsCheck, groundCheck };
    }

    private void UpdateAttractedObject(RaycastHit2D[] groundChecks)
    {
        RaycastHit2D magnetsCheck = groundChecks[0];
        RaycastHit2D groundCheck = groundChecks[1];

        // if a wall is in the way, just skip this iteration
        // does this by checking if both raycasts hit the same collider (meaning no magnet obstructed a wall)
        if (groundCheck.collider != null && groundCheck.collider == magnetsCheck.collider)
        {
            return;
        }

        // assign attracted objects if magnet collides 
        if (magnetsCheck.collider != null)
        {
            // debug line to show successful magnet hit
            Debug.DrawLine(transform.position, magnetsCheck.point, Color.green);

            // undefined attracted object, so just assign
            if (attractedObject != null)
            {
                Vector2 currentDist = (Vector2)transform.position - attractedPoint;
                Vector2 otherDist = (Vector2)transform.position - magnetsCheck.point;

                // if this magnet is farther, skip iteration
                if (otherDist.magnitude > currentDist.magnitude)
                {
                    return;
                }
            }

            // add to attracted objects dictionary
            attractedObject = magnetsCheck.collider.gameObject;
            attractedPoint = magnetsCheck.point;
        }
    }

    void Update()
    {
        // if neutral, do nothing
        if (attractionPolarity == ObjectPolarity.Neutral)
        {
            return;
        }

        CastAttractionCone();

        // apply magnetism to attracted objects
        if (attractedObject != null)
        {
            // drain power
            playerStats.drainPower();

            ApplyMagnetism(attractedObject, attractedPoint);
        }
    }

    private void ApplyMagnetism(GameObject obj, Vector2 attractionPoint)
    {
        // get object properties
        ObjectProperties objProperties = obj.GetComponent<ObjectProperties>();

        // multiplied by -1 so that like polarities repel and opposite polarities attract
        Vector2 attractionVector = attractionPoint - (Vector2)transform.position;
        attractionVector *= -1 * (int)attractionPolarity * (int)objProperties.polarity;

        if (obj.CompareTag("Magnet"))
        {
            // compare weights
            string objPlayerInteraction = ObjectProperties.CompareWeights(properties.weight, objProperties.weight);
            switch (objPlayerInteraction)
            {
                // player and object are equal weight
                case "Equal":
                    ApplyForce(gameObject, attractionVector);
                    //movementController.IgnoreFriction();
                    ApplyForce(obj, -attractionVector);
                    break;

                // player is heavier
                case "Greater":
                    ApplyForce(obj, -attractionVector);
                    break;

                // object is heavier
                case "Less":
                    ApplyForce(gameObject, attractionVector);
                    //movementController.IgnoreFriction();
                    break;
            }
        }
        else
        {
            // anchored magnet; move only the player
            ApplyForce(gameObject, attractionVector);
        }
    }

    private void ApplyForce(GameObject obj, Vector2 attractionVector, float speedMult = 1)
    {
        Vector2 force = speed * speedMult * attractionVector.normalized;

        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();

        //print(obj.name.ToString() + ": " + force.ToString());

        // multiplied by mass to make the "force" acceleration
        rb.AddForce(force * rb.mass);
    }
}
