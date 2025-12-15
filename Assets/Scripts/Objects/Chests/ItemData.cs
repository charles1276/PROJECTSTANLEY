using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public Weight itemWeight;
    [TextArea] public string itemDescription; // might use it later idk // I USED IT LATER
}
