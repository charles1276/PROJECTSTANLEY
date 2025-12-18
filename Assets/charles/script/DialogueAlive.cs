using UnityEngine;

public class DialogueAlive : MonoBehaviour
{
    public GameObject Player;

    public Dialogue dS;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == Player)
        {
            dS.Startdialogue();
            Destroy(gameObject);
        }
    }
}
