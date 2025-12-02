using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class StartDialogue : MonoBehaviour
{
    public Dialogue dialogueScript;
    
    public GameObject trigger;

   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
       Dialogue dialogue = dialogueScript;
         dialogue.Startdialogue();
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
        {

       
    }
}
