using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private int itemID;
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private Sprite itemDescription;
}
