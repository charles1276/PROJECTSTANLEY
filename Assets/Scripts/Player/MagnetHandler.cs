using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.Jobs;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.InputSystem;

public class Vector2Extensions
{
    public static Vector2 AngleToComponents(float angle)
    {
        float rad = angle;
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
            //print("neg");
        }
        if (ctx.canceled && attractionPolarity == ObjectPolarity.Negative)
        {
            attractionPolarity = ObjectPolarity.Neutral;
            //print("neu");
        }
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
            AttractionVector = Vector2Extensions.Rotate(AttractionVector, Vector2Extensions.AngleToComponents(angleDifference));

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

    //private void AssignClickedObject()
    //{
    //    // grab mouse position and raycast
    //    Vector3 mousePosition = Input.mousePosition;
    //    mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
    //    Ray mouseRay = Camera.main.ScreenPointToRay(mousePosition);

    //    // i have no idea what the 1f does here but it works so
    //    RaycastHit2D hit = Physics2D.Raycast(mouseRay.origin, mouseRay.direction, 1f, magnetsLayer);
    //    clickObject = hit ? hit.collider.transform : null; // get clicked object
    //}

    //private void UnassignClickedObject()
    //{
    //    clickObject = null;
    //}

    //private bool CheckGroundObstruction()
    //{
    //    // raycast to check for walls
    //    RaycastHit2D magnetsCheck = Physics2D.Raycast(transform.position, attractionVector.normalized, attractionRange, LayerMask.GetMask("Magnets", "AnchoredMagnets", "Ground"));
    //    RaycastHit2D groundCheck = Physics2D.Raycast(transform.position, attractionVector.normalized, attractionRange, LayerMask.GetMask("Ground"));
    //    Debug.DrawRay(transform.position, attractionVector.normalized * attractionRange, Color.red);

    //    // if a wall is in the way, do nothing
    //    // does this by checking if both raycasts hit the same collider (meaning no magnet obstructed a wall)
    //    if (groundCheck.collider != null && groundCheck.collider == magnetsCheck.collider)
    //    {
    //        //print("Magnet influence blocked by walls.");
    //        UnassignClickedObject();
    //        return true;
    //    }

    //    // reassign clicked object if magnet collides 
    //    // only needed if multiple magnets can be in a line
    //    if (magnetsCheck.collider != null)
    //    {
    //        clickObject = magnetsCheck.collider.transform;
    //        clickPoint = magnetsCheck.point;
    //    }

    //    return false;
    //}

    void Update()
    {
        // if neutral, do nothing
        if (attractionPolarity == ObjectPolarity.Neutral)
        {
            return;
        }

        // put magnetism display logic here later

        //// if no object clicked, do nothing
        //if (clickObject == null)
        //{
        //    return;
        //}

        CastAttractionCone();
        //CheckGroundObstruction();

        //// if distance is greater than attractionRange, do nothing
        //if ((clickPoint - (Vector2)transform.position).magnitude > attractionRange)
        //{
        //    print("too far away, :3");
        //    return;
        //}

        //Vector3 objToPlayerDirection = (clickPoint - (Vector2)transform.position).normalized;

        //Debug.DrawLine(transform.position, clickPoint, Color.green);

        //// if object not within range
        //if (Vector3.Dot(attractionVector.normalized, objToPlayerDirection) < attractionAngleRange)
        //{
        //    //print("uhm,. u need to likeee,. actualy point at th objct.,, >.>");

        //    Debug.Log(Vector3.Dot(attractionVector.normalized, objToPlayerDirection));
        //    Debug.Log(attractionAngleRange);

        //    Debug.DrawRay(transform.position, attractionVector.normalized, Color.yellow);
        //    Debug.DrawRay(transform.position, objToPlayerDirection, Color.blue);

        //    UnassignClickedObject();

        //    return;
        //}

        if (attractedObject != null)
        {
            ApplyMagnetism(attractedObject, attractedPoint);
        }
        //// if clicked object is a loose magnet, apply force based on weight comparison
        //if (clickObject.CompareTag("Magnet"))
        //{
        //    ObjectProperties clickObjectProperties = clickObject.GetComponent<ObjectProperties>();

        //    // multiplied by -1 so that like polarities repel and opposite polarities attract
        //    attractionBehavior = -1 * (int)attractionPolarity * (int)clickObjectProperties.polarity;
        //    //print(attractionPolarity);
        //    //print(clickObjectProperties.polarity);
        //    //print(attractionBehavior);

        //    // compare weights
        //    string objPlayerInteraction = ObjectProperties.CompareWeights(properties.weight, clickObjectProperties.weight);

        //    switch (objPlayerInteraction)
        //    {
        //        // player and object are equal weight
        //        case "Equal":
        //            MovePlayer();
        //            MoveObject();
        //            break;

        //        // player is heavier
        //        case "Greater":
        //            MoveObject();
        //            break;

        //        // object is heavier
        //        case "Less":
        //            MovePlayer();
        //            break;
        //    }
        //}
    }

    private void ApplyMagnetism(GameObject obj, Vector2 attractionPoint)
    {
        // get object properties
        ObjectProperties objProperties = obj.GetComponent<ObjectProperties>();

        // multiplied by -1 so that like polarities repel and opposite polarities attract
        Vector2 attractionVector = attractionPoint - (Vector2)transform.position;
        attractionVector *= -1 * (int)attractionPolarity * (int)objProperties.polarity;

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

    //// apply force to player
    //private void MovePlayer(Vector2 attractionVector, float speedMult = 1)
    //{
    //    Vector2 attractionForce = speed * speedMult * attractionVector.normalized;
    //    ApplyForce(gameObject, attractionForce);
    //}

    //// apply force to clicked object
    //private void MoveObject(GameObject obj, Vector2 attractionVector, float speedMult = 1)
    //{
    //    Vector2 attractionForce = speed * speedMult * attractionVector.normalized;
    //    ApplyForce(obj, attractionForce);
    //}

    private void ApplyForce(GameObject obj, Vector2 attractionVector, float speedMult = 1)
    {
        Vector2 force = speed * speedMult * attractionVector.normalized;

        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();

        //print(obj.name.ToString() + ": " + force.ToString());

        // multiplied by mass to make the "force" acceleration
        rb.AddForce(force * rb.mass);
    }
}
