using UnityEngine;

[CreateAssetMenu(fileName = "Weights", menuName = "Scriptable Objects/Weights")]
public class Weights : ItemData
{
    ItemData Item { get; set; }
    public Weight weightValue;
}
