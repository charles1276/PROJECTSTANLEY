using UnityEngine;

public class openDoor2 : MonoBehaviour
{
    public bool doorOpen = false;
    private Animator anim;
    
    void Awake()
    {
        anim = GetComponent<Animator>();
    }
}
