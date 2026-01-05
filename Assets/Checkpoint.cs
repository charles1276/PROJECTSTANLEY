using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // static variables to hold checkpoint data
    public static Vector2 lastCheckpointPosition;
    public static GameObject[] playerInventory;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // update checkpoint position
            lastCheckpointPosition = transform.position;

            // store player's inventory (assuming PlayerInventory is a component that holds the inventory)
            InventoryManager inventory = collision.GetComponent<InventoryManager>();
            if (inventory != null)
            {
                playerInventory = inventory.inventorySlots;
            }
        }

        print(lastCheckpointPosition);
        print(playerInventory);
    }
}
