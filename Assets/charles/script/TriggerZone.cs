using UnityEngine;
using UnityEngine.Events;

public class TriggerZone : MonoBehaviour
{
    public GameObject targetObject;
     void Start()    
    {
        targetObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        targetObject.SetActive(true);

    }
}
    