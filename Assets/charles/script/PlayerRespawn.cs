using UnityEngine;
        
public class PlayerRespawn : MonoBehaviour
{
    [Tooltip("Assign the GameObject whose position you want to read")]
    public GameObject targetObject;

    // Stored copies of the target's position
   
    public  Vector2 targetPosition2D;

    void Start()
    {
        //this is to check if the targetObject is assigned
        if (targetObject == null)
        {
            Debug.LogError("PlayerRespawn: targetObject is not assigned in the Inspector.");
            return;
        }

        
        targetPosition2D = targetObject.transform.position;

        //this is geting the players position in 2d 
        targetPosition2D = new Vector2(targetPosition2D.x, targetPosition2D.y);

        
    }

     private void OnTriggerEnter2D(Collider2D collision)
     {
        //this is so when the player touches the object the player will respawn
        targetObject.transform.position = new Vector3(targetPosition2D.x, targetPosition2D.y);
    }
}
