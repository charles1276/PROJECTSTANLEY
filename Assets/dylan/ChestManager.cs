using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChestManager : MonoBehaviour
{
    public GameObject storedItem;
    private GameObject itemDisplay;
    private GameObject player;

    [SerializeField] private float interactionProximity;
    private bool withinRange = false;

    // --------------------------------------------------------------
    // INVENTORY MANAGEMENT

    private void SwapObject()
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
            // full inventory + stored object
            // swap the stored object and selected object
            // no stored object
            // place the selected object into the chest
            storedItem = playerInventory.SwapObject(storedItem);
        }
    }

    // --------------------------------------------------------------
    // UNITY METHODS

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // get player reference
        player = GameObject.FindWithTag("Player");

        itemDisplay = Instantiate(storedItem);
        itemDisplay.transform.parent = gameObject.transform;
        itemDisplay.transform.position = gameObject.transform.position + Vector3.up;
        //itemDisplay.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerDistance = player.transform.position - transform.position;

        if (playerDistance.magnitude < interactionProximity)
        {
            withinRange = true;
            //Debug.Log(playerDistance.magnitude);
        }
    }

    public void Interact(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && withinRange)
        {
            SwapObject();
        }
    }
}
