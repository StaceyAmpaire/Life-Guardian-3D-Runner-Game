using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private float loadingDuration = 3f;  // ← Add this (3 seconds)
    private string sceneToLoad;

    private void Start()
    {
        sceneToLoad = PlayerPrefs.GetString("SceneToLoad", "Run");
        StartCoroutine(LoadSceneAsync(sceneToLoad));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        float elapsedTime = 0f;

        while (elapsedTime < loadingDuration || asyncLoad.progress < 0.9f)
        {
            elapsedTime += Time.deltaTime;
            float displayProgress = Mathf.Clamp01(elapsedTime / loadingDuration);
            
            loadingSlider.value = displayProgress;
            loadingText.text = $"Loading... {(int)(displayProgress * 100)}%";
            
            yield return null;
        }

        loadingSlider.value = 1f;
        loadingText.text = "Loading... 100%";
        
        yield return new WaitForSeconds(0.5f);
        
        asyncLoad.allowSceneActivation = true;
        
        yield return null;
    }
}
