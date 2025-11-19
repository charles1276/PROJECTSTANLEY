using UnityEngine;

[CreateAssetMenu(fileName = "Weights", menuName = "Scriptable Objects/Weights")]
public class Weights : ScriptableObject
{
    Item Item { get; set; }
}
