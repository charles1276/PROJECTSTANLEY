using UnityEngine;
using UnityEngine.InputSystem;

public class ChestManager : MonoBehaviour
{
    public GameObject storedItem;
    private GameObject player;

    public GameObject restricted;

    [SerializeField] private float interactionProximity;
    private bool withinRange = false;

    // --------------------------------------------------------------
    // INVENTORY MANAGEMENT

    private void AddObject()
    {
        InventoryManager playerInventory = player.GetComponent<InventoryManager>();

        if (playerInventory.FirstEmptySlot() != -1 && storedItem != null)
        {
            // free inventory + stored object
            // just add the item to the inventory
            playerInventory.AddCollectible(storedItem);
            storedItem = null;
        }
        else
        {

        }
    }

    // --------------------------------------------------------------
    // UNITY METHODS

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // get player reference
        player = GameObject.FindWithTag("Player");

        //itemDisplay.transform.parent = gameObject.transform;
        //itemDisplay.transform.position = gameObject.transform.position + 1.2f * Vector3.up;
        //itemDisplay.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerDistance = player.transform.position - transform.position;
        
        // set withinRange boolean based on the player's distance
        if (playerDistance.magnitude < interactionProximity)
        {
            withinRange = true;
        }
        else
        {
            withinRange = false;
        }

        // set the item display based on the current stored object
        //if (storedItem != null)
        //{
        //    itemDisplay.GetComponent<Image>().sprite = storedItem.GetComponent<ItemInstance>().itemData.itemIcon;
        //}
        //else
        //{
        //    itemDisplay.GetComponent<Image>().sprite = null;
        //}

        // actually display it only when it's within range
        //itemDisplay.SetActive(withinRange);
    }

    public void Interact(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && withinRange)
        {
            AddObject();
        }
    }
}
