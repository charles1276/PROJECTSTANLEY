using UnityEngine;

public class openDoor2 : MonoBehaviour
{
    public bool doorOpen = false;
    private Animator anim;
    private PolygonCollider2D coll;
    
    void Awake()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<PolygonCollider2D>();
    }
    void Update()
    {
        if (doorOpen == true)
        {
            anim.SetBool("ButtonOn", doorOpen);
        }
    }
    void DisableCollider()
    {
        coll.enabled = false;
    }
}
