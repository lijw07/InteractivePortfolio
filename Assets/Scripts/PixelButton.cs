using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class PixelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Button States")]
    [SerializeField] private Color normalColor = new Color(0.8f, 0.8f, 0.8f, 1f);
    [SerializeField] private Color hoverColor = new Color(1f, 1f, 1f, 1f);
    [SerializeField] private Color pressedColor = new Color(0.6f, 0.6f, 0.6f, 1f);
    
    [Header("Animation")]
    [SerializeField] private float scaleOnHover = 1.05f;
    [SerializeField] private float scaleOnPress = 0.95f;
    [SerializeField] private float animationSpeed = 10f;
    
    [Header("Pixel Effect")]
    [SerializeField] private bool usePixelPerfect = true;
    [SerializeField] private int pixelsPerUnit = 16;
    
    private Button button;
    private Image buttonImage;
    private Text buttonText;
    private RectTransform rectTransform;
    private Vector3 originalScale;
    private Color targetColor;
    private float targetScale = 1f;
    
    void Awake()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
        buttonText = GetComponentInChildren<Text>();
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
        targetColor = normalColor;
    }
    
    void Start()
    {
        if (buttonImage != null)
        {
            buttonImage.color = normalColor;
        }
        
        if (usePixelPerfect && buttonImage != null && buttonImage.sprite != null)
        {
            buttonImage.sprite.texture.filterMode = FilterMode.Point;
        }
    }
    
    void Update()
    {
        if (buttonImage != null)
        {
            buttonImage.color = Color.Lerp(buttonImage.color, targetColor, Time.deltaTime * animationSpeed);
        }
        
        if (rectTransform != null)
        {
            Vector3 targetScaleVector = originalScale * targetScale;
            rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, targetScaleVector, Time.deltaTime * animationSpeed);
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button.interactable)
        {
            targetColor = hoverColor;
            targetScale = scaleOnHover;
        }
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (button.interactable)
        {
            targetColor = normalColor;
            targetScale = 1f;
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (button.interactable)
        {
            targetColor = pressedColor;
            targetScale = scaleOnPress;
        }
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        if (button.interactable)
        {
            if (eventData.hovered.Contains(gameObject))
            {
                targetColor = hoverColor;
                targetScale = scaleOnHover;
            }
            else
            {
                targetColor = normalColor;
                targetScale = 1f;
            }
        }
    }
    
    void OnDisable()
    {
        if (buttonImage != null)
        {
            buttonImage.color = normalColor;
        }
        
        if (rectTransform != null)
        {
            rectTransform.localScale = originalScale;
        }
    }
}