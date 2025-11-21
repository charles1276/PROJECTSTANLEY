using UnityEngine;
using UnityEngine.Events;

public class TriggerZone : MonoBehaviour
{
    //this is just for the player to go to the next level or go to the main menu
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
    