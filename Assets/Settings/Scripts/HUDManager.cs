using UnityEngine;

// stats bar class
public class StatsBar
{
    private RectTransform emptyFill;
    private float currentFill;
    private float fillSmoothing; // higher value means faster smoothing

    private int dist;

    // constructor (class initializer)
    public StatsBar(GameObject ui, float smoothing = 5f)
    {
        GameObject fill = ui.transform.Find("fill").gameObject;
        Rect fillSize = fill.GetComponent<RectTransform>().rect;
        dist = (int)(fillSize.width + fillSize.height - 1);

        emptyFill = fill.transform.Find("emptyfill").GetComponent<RectTransform>();
        currentFill = 1f;

        fillSmoothing = smoothing;
    }

    // set the fill amount of the stats bar
    public void setFillAmount(float percentage)
    {
        // smoothly interpolate the fill amount
        currentFill = Mathf.Lerp(currentFill, percentage, Time.deltaTime * fillSmoothing);

        // update the position of the empty fill based on the current fill amount
        emptyFill.anchoredPosition = new Vector3(Mathf.Ceil(dist * currentFill) - dist, 0, 0);
    }
}

public class HUDManager : MonoBehaviour
{
    private GameObject player;

    [Header("UI Elements")]
    [SerializeField] private GameObject staminaBarUI;
    [SerializeField] private GameObject powerBarUI;

    [Header("Stat Bars")]
    public StatsBar staminaBar;
    public StatsBar powerBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // find the player
        player = GameObject.FindGameObjectWithTag("Player");

        // define statistic bars
        staminaBar = new StatsBar(staminaBarUI);
        powerBar = new StatsBar(powerBarUI);
    }

    // Update is called once per frame
    void Update()
    {
        staminaBar.setFillAmount(player.GetComponent<PlayerStats>().getStaminaPercentage());
        powerBar.setFillAmount(player.GetComponent<PlayerStats>().getPowerPercentage());
    }
}
