using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    //[TextArea] private string itemDescription; // might use it later idk
    [SerializeField] private Weight itemWeight;
}
