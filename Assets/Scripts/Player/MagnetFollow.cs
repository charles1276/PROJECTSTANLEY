using UnityEngine;

public class MagnetFollow : MonoBehaviour
{
    [SerializeField] Transform Magnet;

    Vector2 direction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Get mouse position in world space
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePos - (Vector2)Magnet.position;
        //Get Blue and Red positions

        //Flips player based on mouse position
        if (mousePos.x <= transform.position.x)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, 1);
            Magnet.transform.localScale = new Vector3(-0.68f, 0.44535f, 1f);
        }
        else if (mousePos.x > transform.position.x)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, 1);
            Magnet.transform.localScale = new Vector3(0.68f, 0.44535f, 1f);
        }
        //Magnet faces mouse
        FaceMouse();
    }

    void FaceMouse()
    {
        //Magnet.transform.right = direction;
        if (transform.localScale.x == 1)
        {
            Magnet.transform.right = new Vector2(Mathf.Abs(direction.x), direction.y);
        }
         if (transform.localScale.x == -1)
         {
            Magnet.transform.right = new Vector2(-Mathf.Abs(direction.x), direction.y);
        }

    }
}
