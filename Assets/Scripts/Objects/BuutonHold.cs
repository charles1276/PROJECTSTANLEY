using UnityEngine;

public class BuutonHold : MonoBehaviour
{
    public Sprite spro;
    public Sprite spru;

    private SpriteRenderer spriteRenderer;

    public openDoor2 doorScript;

    public Activategroungmagnet GroundMagnet;

    private void Awake()
    {         
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Magnet"))
        {
            Debug.Log("Button Pressed!");
            // Add additional button press logic here
            spriteRenderer.sprite = spro;
            doorScript.doorOpen = true;
            GroundMagnet.Active = true;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Magnet"))
        {
            Debug.Log("Button Released!");
            // Add additional button release logic here
            spriteRenderer.sprite = spru;
            doorScript.doorOpen = false;
            GroundMagnet.Active = false;
        }
    }
}
