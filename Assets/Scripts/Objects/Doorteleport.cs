using UnityEngine;

public class DoorTeleport : MonoBehaviour
{
    public Vector3 doorPosition;
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.position = doorPosition;
        }
    }
}
