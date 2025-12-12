using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ItemInstance : MonoBehaviour
{
    public ItemData itemData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = itemData.itemIcon;
    }
}
