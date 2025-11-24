using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class TriggerZoneForObject : MonoBehaviour
{
    public GameObject targetObject;
    private bool dooropen=false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
       dooropen=true;
    }
    private void Update()
    {
        if (dooropen = true)
        {
            if (targetObject.transform.position.y <= 5f)
            {
                Vector3 pos = targetObject.transform.position;
                pos.y += .5f;
                targetObject.transform.position = pos;
            }
        }
    }
}
