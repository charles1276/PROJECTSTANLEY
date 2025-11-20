using UnityEngine;

// stats bar class
public class StatsBar
{
    private RectTransform emptyFill;
    private float currentFill;
    private float fillSmoothing; // higher value means faster smoothing

    private int dist = 37;

    // constructor (class initializer)
    public StatsBar(GameObject ui, float smoothing = 5f)
    {
        emptyFill = ui.transform.Find("emptyfill").GetComponent<RectTransform>();
        currentFill = 1f;

        fillSmoothing = smoothing;
    }

    // set the fill amount of the stats bar
    public void setFillAmount(float percentage)
    {
        // smoothly interpolate the fill amount
        currentFill = Mathf.Lerp(currentFill, percentage, Time.deltaTime * fillSmoothing);

        // update the position of the empty fill based on the current fill amount
        emptyFill.localPosition = new Vector3(dist * currentFill, 0, 0);
    }
}

public class HUDManager : MonoBehaviour
{
    [SerializeField] private GameObject staminaBarUI;
    [SerializeField] private GameObject powerBarUI;

    public StatsBar staminaBar;
    public StatsBar powerBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        staminaBar = new StatsBar(staminaBarUI);
        powerBar = new StatsBar(powerBarUI);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
