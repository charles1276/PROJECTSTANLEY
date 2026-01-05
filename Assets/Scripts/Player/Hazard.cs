using UnityEngine;
using UnityEngine.SceneManagement;

public class Hazard : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // reload the current scene to reset the game state
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            // set player to the last checkpoint position
            other.transform.position = Checkpoint.lastCheckpointPosition;
            other.gameObject.GetComponent<InventoryManager>().inventorySlots = Checkpoint.playerInventory;
        }
    }
}
