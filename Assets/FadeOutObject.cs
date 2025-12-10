using UnityEngine;

public class FadeOutObject : MonoBehaviour
{
    private Quaternion endRotation;
    private Quaternion startRotation;
    private float startTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startRotation = transform.rotation;
        endRotation = Random.rotation;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Lerp(startRotation, endRotation, Mathf.Pow(Time.time - startTime, 2));
    }
}
