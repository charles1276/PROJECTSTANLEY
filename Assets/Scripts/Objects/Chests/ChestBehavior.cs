using UnityEngine;

public class ChestBehavior : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] protected GameObject restrictedIcon;

    protected GameObject player;
    [SerializeField] private float interactionProximity;
    private GameObject interactionDisplay;
    [SerializeField] private Sprite interactionIcon;
    protected bool withinRange = false;

    public virtual void Interact()
    {
        // to be overridden
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // get player reference
        player = GameObject.FindWithTag("Player");

        // interaction
        interactionDisplay = new GameObject();
        SpriteRenderer idsr = interactionDisplay.AddComponent<SpriteRenderer>();
        idsr.sprite = interactionIcon;
        idsr.sortingOrder = 3;
        interactionDisplay.transform.parent = gameObject.transform;
        interactionDisplay.transform.position = gameObject.transform.position + 1.2f * Vector3.up;
        interactionDisplay.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerStats ps = player.GetComponent<PlayerStats>();
        Vector2 playerDistance = player.transform.position - transform.position;

        // set withinRange boolean based on the player's distance
        if (playerDistance.magnitude < interactionProximity)
        {
            ps.UpdateSelectedChest(gameObject);
        }

        // actually display it only when it's within range
        interactionDisplay.SetActive(ps.selectedChest == gameObject);
    }
}
