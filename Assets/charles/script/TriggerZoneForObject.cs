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
            if (targetObject.transform.localScale.x <= 1.8f)
            {
                Vector3 localScale = targetObject.transform.localScale;
                localScale.x += .00125f;
                targetObject.transform.localScale = localScale;
            }
            if (targetObject.transform.position.x <= 108.25f)
            {
                Vector3 pos = targetObject.transform.position;
                pos.x += .000625f;
                targetObject.transform.position = pos;

                Vector3 post = targetObject.transform.position;
                post.z = 1f;
                targetObject.transform.position = post;
            }
        }
    }
}
