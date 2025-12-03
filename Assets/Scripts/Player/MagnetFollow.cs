using UnityEngine;

public class MagnetFollow : MonoBehaviour
{
    public Transform Magnet;

    [SerializeField] Transform projection;

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
        //Flips player based on mouse position
        if (mousePos.x <= transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (mousePos.x > transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
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
