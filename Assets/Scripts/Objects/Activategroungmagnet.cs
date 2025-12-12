using UnityEngine;

public class Activategroungmagnet : MonoBehaviour
{
    public bool Active = false;
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
        
    }
    void Update()
    {
        if(Active == true)
        {
            gameObject.tag = "Magnet";
            anim.SetBool("ButtonPress", true);
        }
        if(Active == false)
        {
            gameObject.tag = "Untagged";
            anim.SetBool("ButtonPress", false);
        }
    }
}
