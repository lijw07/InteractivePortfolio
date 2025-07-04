using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button startButton;
    [SerializeField] private Text welcomeText;
    [SerializeField] private string mainSceneName = "MainGame";
    
    [Header("Animation Settings")]
    [SerializeField] private float fadeInDuration = 1.5f;
    [SerializeField] private AnimationCurve fadeInCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    private CanvasGroup canvasGroup;
    
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }
        
        if (welcomeText != null)
        {
            welcomeText.text = "Welcome to Jai's Portfolio";
        }
        
        StartCoroutine(FadeInUI());
    }
    
    private System.Collections.IEnumerator FadeInUI()
    {
        canvasGroup.alpha = 0f;
        float elapsedTime = 0f;
        
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / fadeInDuration;
            canvasGroup.alpha = fadeInCurve.Evaluate(normalizedTime);
            yield return null;
        }
        
        canvasGroup.alpha = 1f;
    }
    
    private void OnStartButtonClicked()
    {
        StartCoroutine(TransitionToMainScene());
    }
    
    private System.Collections.IEnumerator TransitionToMainScene()
    {
        if (startButton != null)
        {
            startButton.interactable = false;
        }
        
        float elapsedTime = 0f;
        float fadeDuration = 0.5f;
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = 1f - (elapsedTime / fadeDuration);
            canvasGroup.alpha = normalizedTime;
            yield return null;
        }
        
        if (!string.IsNullOrEmpty(mainSceneName))
        {
            SceneManager.LoadScene(mainSceneName);
        }
        else
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
    
    void OnDestroy()
    {
        if (startButton != null)
        {
            startButton.onClick.RemoveListener(OnStartButtonClicked);
        }
    }
}