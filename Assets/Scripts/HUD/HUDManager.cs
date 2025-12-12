using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

public class MagnetismUI
{
    private GameObject uiElement;
    private Sprite[] sprites;
    private ObjectPolarity currentPolarity;

    // constructor (class initializer)
    public MagnetismUI(GameObject ui, Sprite[] sprites)
    {
        uiElement = ui;
        this.sprites = sprites;
    }

    // set the sprite of the magnetism UI element according to the object's polarity
    public void SetPolarity(ObjectPolarity objectPolarity)
    {
        switch (objectPolarity)
        {
            case ObjectPolarity.Positive:
                uiElement.GetComponent<Image>().sprite = sprites[0];
                break;

            case ObjectPolarity.Neutral:
                // previously positive
                if (currentPolarity == ObjectPolarity.Positive)
                {
                    uiElement.GetComponent<Image>().sprite = sprites[1];
                }

                // previously negative
                else if (currentPolarity == ObjectPolarity.Negative)
                {
                    uiElement.GetComponent<Image>().sprite = sprites[3];
                }

                // no previous polarity
                else
                {
                    uiElement.GetComponent<Image>().sprite = sprites[1];
                }
                   
                break;

            case ObjectPolarity.Negative:
                uiElement.GetComponent<Image>().sprite = sprites[2];
                break;
        }
        
        currentPolarity = objectPolarity;
    }
}

public class HUDManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject player;

    [Header("UI Elements")]
    [SerializeField] private GameObject staminaBarUI;
    [SerializeField] private GameObject powerBarUI;

    [Header("Stat Bars")]
    public StatsBar staminaBar;
    public StatsBar powerBar;

    [Header("Magnet Indicator Sprites")]
    [SerializeField] private GameObject indicatorUI; // negative, neutral, positive

    [Header("Magnetism Indicator")]
    public MagnetismUI magnetismIndicator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // find the player
        player = GameObject.FindGameObjectWithTag("Player");

        // define statistic bars
        staminaBar = new StatsBar(staminaBarUI);
        powerBar = new StatsBar(powerBarUI);

        // grab sprite array for magnetism indicator
        Sprite[] magnetismSprites = indicatorUI.GetComponent<StateStorage>().spriteList;

        // define magnetism indicator
        magnetismIndicator = new MagnetismUI(indicatorUI, magnetismSprites);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        staminaBar.setFillAmount(playerStats.stamina.GetPercentage());
        powerBar.setFillAmount(playerStats.power.GetPercentage());
    }

    public void updateMagnetismIndicator(ObjectPolarity polarity)
    {
        magnetismIndicator.SetPolarity(polarity);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        print("stupid");
        if (eventData.pointerEnter.name.Contains("Slot")) {

        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
