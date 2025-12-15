using UnityEngine;

public class Doorteleport : MonoBehaviour
{
    public publicGameObject playerPosition;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.position = playerPosition;
        }
    }
}
