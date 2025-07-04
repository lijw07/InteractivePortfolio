using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameSceneManager : MonoBehaviour
{
    private static GameSceneManager instance;
    
    [Header("Transition Settings")]
    [SerializeField] private float transitionDuration = 0.5f;
    
    public static GameSceneManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameSceneManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("GameSceneManager");
                    instance = go.AddComponent<GameSceneManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return instance;
        }
    }
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }
    
    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadSceneAsync(sceneIndex));
    }
    
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        yield return new WaitForSeconds(transitionDuration);
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    
    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        yield return new WaitForSeconds(transitionDuration);
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    
    public void RestartCurrentScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}