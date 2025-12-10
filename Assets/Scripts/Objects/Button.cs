using UnityEngine;

public class Button : MonoBehaviour
{
    public Sprite spro;

    private SpriteRenderer spriteRenderer;

    public openDoor2 doorScript;

    void Awake()
    {         
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Button Pressed!");
            // Add additional button press logic here
            spriteRenderer.sprite = spro;
            doorScript.doorOpen = true;
        }
    }
}
