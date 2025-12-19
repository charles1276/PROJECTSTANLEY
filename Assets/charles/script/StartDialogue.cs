using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class StartDialogue : MonoBehaviour
{
    public Dialogue dialogueScript;
    public Dialogue dialogueScript2;
    public GameObject trigger;
    public GameObject trigger2;   
    public GameObject trigger3;
    public bool Enter = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
       Dialogue dialogue = dialogueScript;
         dialogue.Startdialogue();
        
    }
    private void FixedUpdate()
    {
        if (Enter)
        {
            trigger2.SetActive(true);
            Dialogue dialogue = dialogueScript2;
            dialogue.Startdialogue();
            Destroy(trigger3);
        }
    }
}
