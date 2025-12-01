using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private int itemID; // useful ??
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private Sprite itemDescription;
}
