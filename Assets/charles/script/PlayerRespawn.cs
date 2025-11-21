using UnityEngine;
        
public class PlayerRespawn : MonoBehaviour
{
    [Tooltip("Assign the GameObject whose position you want to read")]
    public GameObject targetObject;

    // Stored copies of the target's position
   
    public  Vector2 targetPosition2D;

    void Start()
    {
        if (targetObject == null)
        {
            Debug.LogError("PlayerRespawn: targetObject is not assigned in the Inspector.");
            return;
        }

        
        targetPosition2D = targetObject.transform.position;

        // Convert to Vector2 if you only need X and Y (useful for 2D games)
        targetPosition2D = new Vector2(targetPosition2D.x, targetPosition2D.y);

        // Example usage: move this object to the target position (uncomment if needed)
        // transform.position = targetPosition3D;
    }

     private void OnTriggerEnter2D(Collider2D collision)
     {
        targetObject.transform.position = new Vector3(targetPosition2D.x, targetPosition2D.y);
    }
}
