using System.Collections;
using UnityEngine;

public class FadeOutObject : MonoBehaviour
{
    [SerializeField] private bool pureFading = false;

    private float rotationDifference;
    private float startTime;

    private Color startColor;
    private Color endColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // random value between -1 to 1 (times 10)
        rotationDifference = (2 * Random.value - 1) * 5;

        startTime = Time.time;

        startColor = GetComponent<SpriteRenderer>().color;
        endColor = new Color(startColor.r, startColor.g, startColor.b, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float timet = Time.time - startTime;

        if (!pureFading)
        {
            transform.Rotate(new Vector3(0, 0, rotationDifference * timet));
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f * timet, 0);
        }

        GetComponent<SpriteRenderer>().color = Color.Lerp(startColor, endColor, Mathf.Pow(timet, 2));

        // destroy object once it becomes invisible
        if (GetComponent<SpriteRenderer>().color == endColor)
        {
            Destroy(gameObject);
        }
    }
}
