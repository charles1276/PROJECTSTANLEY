using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct DialoguePiece
{
    public string id;
    public string name;
    [TextArea] public string Dialogue;
}
public class Dialogue : MonoBehaviour
{
    public List<DialoguePiece> dialogue;
    public float textSpeed = 0.1f;
    public GameObject image;
    public GameObject image2;

    public TMPro.TMP_Text dialogueName;
    public TMPro.TMP_Text dialogueText;
    private int dialogueIndex = 0;
    // add static current dialogue to keep track of which dialogue is being used
    //private Dialogue currentDialogue;
    public string DialogueId;
    public void Startdialogue()
    {
        ShowImage(dialogue[dialogueIndex]);
        StopAllCoroutines();
        dialogueIndex = 0; // start at first piece
        gameObject.SetActive(true);
        if (dialogue != null && dialogue.Count > 0)
            StartCoroutine(writedialguePiece(dialogue[dialogueIndex]));
    }

    public void EndDialogue()
    {
        gameObject.SetActive(false);
        StopAllCoroutines();
    }

    //dvance to the next piece (or end)
    public void NextDialogueOrStop()
    {

        ++dialogueIndex;
        if (dialogueIndex >= dialogue.Count)
        {
            Debug.Log("End of Dialogue");
            EndDialogue();
            return;
        }

        StopAllCoroutines();
        StartCoroutine(writedialguePiece(dialogue[dialogueIndex]));
    }   

    public IEnumerator writedialguePiece(DialoguePiece dialogue)
    {
        // write the dialogue piece letter-by-letter with a small delay
        dialogueName.SetText(dialogue.name);
        dialogueText.text = "";

        for (int i = 0; i < dialogue.Dialogue.Length; i++)
        {
            dialogueText.text += dialogue.Dialogue[i];
            yield return new WaitForSeconds(textSpeed);
        }

       //proceed to the next piece after a short pause
        yield return new WaitForSeconds(1f);
        NextDialogueOrStop();
        
        
    }
    public void ShowImage(DialoguePiece dialogue)
    {
        DialogueId = dialogue.id;
        if (DialogueId == "s")
        {
            image.SetActive(true);
            image2.SetActive(false);
        }
        if (DialogueId == "g")
        {
            image.SetActive(false);
            image2.SetActive(true);
        }
    }
    public void Update()
    {
        ShowImage(dialogue[dialogueIndex]);


    }
}
