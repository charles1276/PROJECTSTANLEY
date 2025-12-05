using UnityEngine;
using UnityEngine.UI;

public class ObjectHovering : MonoBehaviour
{
    [Header("Weight Display Settings")]
    [SerializeField] private GameObject weightDisplay;
    [SerializeField] private float hoverHeight = 2f;
    [SerializeField] private float hoverWidth = 2f;
    [Tooltip("Speed at which the weight display appears.")]
    [SerializeField] private float displaySpeed = 5f;

    private Vector2 weightTextureSize;
    private Vector2 weightWorldTextureSize;
    private float fillPercentage = 0;

    private Sprite[] weightDisplayLevels;

    [Header("Player Magnet")]
    private MagnetHandler magnetHandler;

    // check if object is outside camera view
    private bool IsObjectOutsideCamera(Vector3 objectPosition)
    {
        Camera targetCamera = Camera.main;

        // convert world position to viewport position
        Vector3 viewportPoint = targetCamera.WorldToViewportPoint(objectPosition);

        // check if the object is behind the camera (z is negative)
        if (viewportPoint.z < 0)
        {
            return true;
        }

        // check if the object's x or y coordinates are outside the [0, 1] range
        if (viewportPoint.x < 0 || viewportPoint.x > 1 || viewportPoint.y < 0 || viewportPoint.y > 1)
        {
            return true;
        }

        // if none of the above are true, the center of the object is within the camera's view
        return false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // get weight display size
        weightTextureSize = weightDisplay.GetComponent<RectTransform>().sizeDelta;

        // get weight display size on the screen
        weightWorldTextureSize = weightTextureSize * GetComponent<RectTransform>().lossyScale;

        // load weight display sprites
        weightDisplayLevels = weightDisplay.GetComponent<StateStorage>().spriteList;

        // get magnet handler reference
        magnetHandler = GameObject.FindGameObjectWithTag("Player").GetComponent<MagnetHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;

        Ray mouseRay = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit2D mouseRaycast = Physics2D.Raycast(mouseRay.origin, mouseRay.direction, 1f, LayerMask.GetMask("Magnets"));

        // hovering over object with neutral polarity
        if (mouseRaycast.collider != null && magnetHandler.attractionPolarity == ObjectPolarity.Neutral)
        {
            // move object to mouse position
            Vector2 mouseWorldPosition = mouseRaycast.point;
            transform.position = mouseWorldPosition + Vector2.up * hoverHeight + Vector2.right * hoverWidth;

            // hide weight display if mouse is outside camera view
            if (IsObjectOutsideCamera(mouseWorldPosition))
            {
                return;
            }

            fillPercentage = Mathf.Lerp(fillPercentage, 1, Time.deltaTime * displaySpeed);

            // position weight display offset from the mouse position
            Vector2 weightTextureOffset = weightTextureSize / 2;

            if (IsObjectOutsideCamera(mouseWorldPosition + (hoverHeight + weightWorldTextureSize.y) * Vector2.up))
            {
                // flip weight display downwards
                weightTextureOffset.y *= -1;

                // adjust object position downwards
                transform.position += 2 * hoverHeight * Vector3.down;
            }

            if (IsObjectOutsideCamera(mouseWorldPosition + (hoverWidth + weightWorldTextureSize.x) * Vector2.right))
            {
                // flip weight display to the left
                weightTextureOffset.x *= -1;

                // adjust object position leftwards
                transform.position += 2 * hoverWidth * Vector3.left;
            }

            weightDisplay.GetComponent<RectTransform>().localPosition = weightTextureOffset;

            // set weight display sprite according to weight
            mouseRaycast.collider.gameObject.TryGetComponent<ObjectProperties>(out ObjectProperties weightStorage);
            weightDisplay.GetComponent<Image>().sprite = weightDisplayLevels[(int)weightStorage.weight];
        }
        else
        {
            fillPercentage = Mathf.Lerp(fillPercentage, 0, Time.deltaTime * displaySpeed);
        }

        // set weight display fill amount
        float onPixelFill = Mathf.Round(fillPercentage * weightTextureSize.x) / weightTextureSize.x;
        weightDisplay.GetComponent<Image>().fillAmount = onPixelFill;
    }
}
