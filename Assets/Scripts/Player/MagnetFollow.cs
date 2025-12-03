using UnityEngine;

public class MagnetFollow : MonoBehaviour
{
    public Transform Magnet;

    Vector2 direction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePos - (Vector2)Magnet.position;
        FaceMouse();
    }

    void FaceMouse()
    {
        //Magnet.transform.right = direction;
        if (transform.parent.localScale.x == -1)
        {
            Magnet.transform.right = new Vector2(Mathf.Abs(direction.x), direction.y);
        }
        if (transform.parent.localScale.x == 1)
        {
            Magnet.transform.right = new Vector2(-Mathf.Abs(direction.x), direction.y);
        }

    }
}
