using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable] 
public struct DialoguePiece
{
    public string name;
   [TextArea] public string Dialogue;
}
public class Dialogue : MonoBehaviour
{
    public List<DialoguePiece> dialogue;
}
