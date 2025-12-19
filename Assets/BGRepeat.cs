using UnityEngine;

public class BGRepeat : MonoBehaviour
{
    private Transform player;

    // round to nearest multiple of n
    // readable code
    private int roundToNearestMultiple(float value, int n)
    {
        return Mathf.RoundToInt(value / n) * n;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // find player
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // repeat background based on player position
        float newX = roundToNearestMultiple(player.position.x, (int)transform.localScale.x);
        float newY = roundToNearestMultiple(player.position.y, (int)transform.localScale.y);

        // set position
        transform.position = new Vector3(newX, newY, transform.position.z);
    }
}
