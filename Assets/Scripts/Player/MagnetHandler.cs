using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
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

    //Attraction and Repel Projection
    [SerializeField] GameObject BlueRepel;
    [SerializeField] GameObject RedAttract;

    //Projection Animators
    [SerializeField] Animator BlueAnim;
    [SerializeField] Animator RedAnim;

    // polarity
    public ObjectPolarity attractionPolarity;

    // reference to object properties
    private ObjectProperties properties;
    private PlayerStats playerStats;
    private Dictionary<string, AudioSource> magnetSFX;

    // hud manager
    private HUDManager hudManager;

    void Start()
    {
        properties = gameObject.GetComponent<ObjectProperties>();
        playerStats = gameObject.GetComponent<PlayerStats>();

        hudManager = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDManager>();

        magnetSFX = transform.Find("Magnet").GetComponent<AudioStorage>().audioSources;
        print(magnetSFX);
    }

    private void ActivateMagnetSFX()
    {
        magnetSFX["Startup"].PlayScheduled(0.1);
        magnetSFX["FaintStartup"].Play();
        magnetSFX["Hum"].Play();
    }

    private void DeactivateMagnetSFX()
    {
        magnetSFX["Hum"].Stop();
        magnetSFX["Close"].PlayScheduled(0.1);
    }

    // input action for attracting
    public void Attract(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            attractionPolarity = ObjectPolarity.Positive;
            //print("pos");
            //if (BlueAnim.GetBool("IsRepeling") == false)
            //{
            //    RedAttract.SetActive(true);
            //}

            // play sfx
            ActivateMagnetSFX();
        }
        if (ctx.canceled && attractionPolarity == ObjectPolarity.Positive)
        {
            attractionPolarity = ObjectPolarity.Neutral;
            //print("neu");
                RedAnim.SetBool("IsAttracting", false);

            // play sfx
            DeactivateMagnetSFX();
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
            //if (RedAnim.GetBool("IsAttracting") == false)
            //{
            //    BlueRepel.SetActive(true);
            //}

            // play sfx
            ActivateMagnetSFX();
        }
        if (ctx.canceled && attractionPolarity == ObjectPolarity.Negative)
        {
            attractionPolarity = ObjectPolarity.Neutral;
            //print("neu");
                BlueAnim.SetBool("IsRepeling", false);

            // play sfx
            DeactivateMagnetSFX();
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
        RaycastHit2D middleCheck = FindGroundCheckVectors(middleAttractionVector);

        // check middle vector first
        UpdateAttractedObject(middleCheck);

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
            RaycastHit2D check = FindGroundCheckVectors(AttractionVector);

            // turn vector for next iteration
            AttractionVector = Vector2Extensions.Rotate(AttractionVector, angleDifferenceComponents);

            // check raycast results
            UpdateAttractedObject(check);
        }
    }

    private RaycastHit2D FindGroundCheckVectors(Vector2 vector)
    {
        // raycast to check for walls
        RaycastHit2D magnetCheck = Physics2D.Raycast(transform.position, vector.normalized, attractionRange, LayerMask.GetMask("Magnets", "AnchoredMagnets", "Ground"));
        Debug.DrawRay(transform.position, vector.normalized * attractionRange, Color.red);

        return magnetCheck;
    }

    private void UpdateAttractedObject(RaycastHit2D magnetCheck)
    {
        // assign attracted objects if magnet collides 
        if (magnetCheck.collider != null && magnetCheck.collider.CompareTag("Magnet"))
        {
            // debug line to show successful magnet hit
            Debug.DrawLine(transform.position, magnetCheck.point, Color.green);

            // undefined attracted object, so just assign
            if (attractedObject != null)
            {
                Vector2 currentDist = (Vector2)transform.position - attractedPoint;
                Vector2 otherDist = (Vector2)transform.position - magnetCheck.point;

                // if this magnet is farther, skip iteration
                if (otherDist.magnitude > currentDist.magnitude)
                {
                    return;
                }
            }

            // add to attracted objects dictionary
            attractedObject = magnetCheck.collider.gameObject;
            attractedPoint = magnetCheck.point;
        }
    }

    void Update()
    {
        // if neutral, do nothing
        if (attractionPolarity == ObjectPolarity.Neutral)
        {
            return;
        }

        // if out of power, do nothing
        if (!playerStats.power.CanUse())
        {
            return;
        }

        CastAttractionCone();

        // apply magnetism to attracted objects
        if (attractedObject != null)
        {
            // drain power
            playerStats.power.Drain();

            ApplyMagnetism(attractedObject, attractedPoint);
        }
        if (attractionPolarity == ObjectPolarity.Negative)
        {
            BlueRepel.SetActive(true);
        }
        else
        {
            BlueAnim.SetBool("IsRepeling", false);
        }
        if (attractionPolarity == ObjectPolarity.Positive)
        {
            RedAttract.SetActive(true);
        }
        else
        {
            RedAnim.SetBool("IsAttracting", false);
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
