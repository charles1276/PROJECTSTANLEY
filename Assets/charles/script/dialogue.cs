using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

[Serializable]
public struct DialoguePiece
{
    public string name;
    [TextArea] public string Dialogue;
}
public class Dialogue : MonoBehaviour
{
    public List<DialoguePiece> dialogue;
    public float textSpeed = 0.1f;
    public TMPro.TMP_Text dialogueName;
    public TMPro.TMP_Text dialogueText;
    private int dialogueIndex = 0;

    public void Startdialogue()
    {
        gameObject .SetActive(true);
        StartCoroutine(writedialguePiece(dialogue[0]));
    }
    public void EndDialogue()
    {
        gameObject.SetActive(false);
    }
    public void NextdialogueOrStop()
    {
        ++dialogueIndex;
        if (dialogueIndex >= dialogue.Count)
        {
            EndDialogue();
            return;
        }
        StartCoroutine(writedialguePiece(dialogue[dialogueIndex]));
        
    }

    public IEnumerator writedialguePiece(DialoguePiece dialogue)
    {
        dialogueName.SetText(dialogue.name);
        dialogueText.text = "";
        for (int i = 0; i < dialogue.Dialogue.Length; i++)
        {
            
            dialogueText.text += dialogue.dialogue[i];
            yield return new WaitForSeconds(textSpeed);
        }
        yield return null;
    }
}
