using UnityEngine;

public class EndScene : MonoBehaviour
{
    public GameObject robot;
    public  Vector2 robotPosition2D;
    public GameObject guard;
    public Vector2 guardPosition2D;
    public Vector3 guardRotation2D;
    public Sprite guardsprite;
    public SpriteRenderer spriteRenderer;
    public Animator guardAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    
        if (robot == null)
        {
            Debug.LogError("PlayerRespawn: targetObject is not assigned in the Inspector.");
            return;
        }
        if (guard == null)
        {
            Debug.LogError("PlayerRespawn: targetObject is not assigned in the Inspector.");
            return;
        }
        
        robotPosition2D = robot.transform.position;
        guardPosition2D = guard.transform.position;
        //this is geting the players position in 2d 
        robotPosition2D = new Vector2(robotPosition2D.x, robotPosition2D.y);
        guardPosition2D = new Vector2(guardPosition2D.x, guardPosition2D.y);
        guardRotation2D = new Vector3(guard.transform.rotation.x, guard.transform.rotation.y, guard.transform.rotation.z);
        // guard.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        guardRotation2D.z = 0f;
        robotPosition2D = robot.transform.position;
        if (robotPosition2D.x >= 25.36f)
        {
            robotPosition2D.x -= 0.006f;
            
        }
        if (robotPosition2D.x <= 25.36f & robotPosition2D.y <= 0.2f & robotPosition2D.x >= 24.42f)
        {
            robotPosition2D.x -= 0.006f;
            robotPosition2D.y += 0.009f;
       
        }
        else if (robotPosition2D.x >= 22f)
        {
            robotPosition2D.x -= 0.006f;
        }
        robot.transform.position = new Vector3(robotPosition2D.x, robotPosition2D.y);
        if (robotPosition2D.x <= 27f & guardPosition2D.x >= 25.9f)
        {
            //guard.SetActive(true);
            guardPosition2D.x -= 0.006f;
            
        }
        if (guardPosition2D.x <= 26f )
        {
           spriteRenderer = guard.GetComponent<SpriteRenderer>();
              spriteRenderer.sprite = guardsprite;
            guardAnimator = guard.GetComponent<Animator>();
            guardAnimator.enabled = false;
            guardRotation2D.y = 0f;
        }
        guardRotation2D.z = 0f;
        guard.transform.position = new Vector3(guardPosition2D.x, guardPosition2D.y);
    }


}
