using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public struct DialoguesPiece
{
    public string name;
    [TextArea] public string Dialogue;
}
public class Dialogues : MonoBehaviour
{
    public List<DialoguePiece> dialogue;

    public TMPro.TMP_Text dialogueName;
    public TMPro.TMP_Text dialogueText;

    public void Startdialogue()
    {

    }
}