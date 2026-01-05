using UnityEngine;
using UnityEngine.SceneManagement;

public class Hazard : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // set player to the last checkpoint position
            other.transform.position = Checkpoint.lastCheckpointPosition;
            other.gameObject.GetComponent<InventoryManager>().inventorySlots = Checkpoint.playerInventory;

            // Reload the current scene to respawn the player at the last checkpoint
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
