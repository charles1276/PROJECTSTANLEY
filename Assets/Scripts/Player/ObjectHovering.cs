using UnityEngine;

public class ObjectHovering : MonoBehaviour
{
    // check if object is outside camera view
    private bool IsObjectOutsideCamera(Vector3 objectPosition)
    {
        Camera targetCamera = Camera.main;

        // Convert world position to viewport position
        Vector3 viewportPoint = targetCamera.WorldToViewportPoint(objectPosition);

        // Check if the object is behind the camera (z is negative)
        if (viewportPoint.z < 0)
        {
            return true;
        }

        // Check if the object's x or y coordinates are outside the [0, 1] range
        if (viewportPoint.x < 0 || viewportPoint.x > 1 || viewportPoint.y < 0 || viewportPoint.y > 1)
        {
            return true;
        }

         If none of the above are true, the center of the object is within the camera's view
        return false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
   {
        Vector3 mousePosition = Input.mousePosition;

        Ray mouseRay = Camera.main.ScreenPointToRay(mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(mouseRay, out hitInfo))
        {
            Transform objectHit = hitInfo.transform;
            Debug.Log("Hovering over: " + objectHit.name);
        }
    }
}
