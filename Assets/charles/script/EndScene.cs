using UnityEngine;

public class EndScene : MonoBehaviour
{
    public GameObject robot;
    public  Vector2 robotPosition2D;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    
        if (robot == null)
        {
            Debug.LogError("PlayerRespawn: targetObject is not assigned in the Inspector.");
            return;
        }


        robotPosition2D = robot.transform.position;

        //this is geting the players position in 2d 
        robotPosition2D = new Vector2(robotPosition2D.x, robotPosition2D.y);

        
    }

    // Update is called once per frame
    void Update()
    {
       robotPosition2D = robot.transform.position;
        if (robotPosition2D.x >= 25.36f)
        {
            robotPosition2D.x -= 0.008f;
            
        }
        if (robotPosition2D.x <= 25.36f & robotPosition2D.y <= 0.07f)
        {
            robotPosition2D.x -= 0.008f;
            robotPosition2D.y += 0.008f;
       
        }
        robot.transform.position = new Vector3(robotPosition2D.x, robotPosition2D.y);
    }
}
