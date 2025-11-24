using UnityEngine;
using UnityEngine.Events;

public class TriggerZoneForObject : MonoBehaviour
{
    public GameObject targetObject;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetObject == null)
        {
            Vector3 pos = targetObject.transform.position;
            pos.y += 2f;
            targetObject.transform.position = pos;
        }
    }
}
