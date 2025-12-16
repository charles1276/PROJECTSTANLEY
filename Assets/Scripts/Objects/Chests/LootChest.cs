using UnityEngine;
using UnityEngine.InputSystem;

public class LootChest : ChestBehavior
{
    [Header("Items")]
    public GameObject storedItem;

    public openDoor2 doorToOpen;

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

            // item notif
            GetComponent<AudioSource>().Play();

            doorToOpen.doorOpen = true;
        }
        else
        {
            GameObject restricted = Instantiate(restrictedIcon);
            restricted.transform.position = transform.position;
        }
    }

    // --------------------------------------------------------------
    // UNITY METHODS

    public void Interact(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && withinRange)
        {
            AddObject();
        }
    }
}
