using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AsyncSceneLoader : MonoBehaviour
{
    [SerializeField] string sceneToLoad;

    [Header("UI ref")]
    [SerializeField] private GameObject loadingScreenCanvas;
    [SerializeField] private Image loadingWheel;
    [SerializeField] private TextMeshProUGUI progressText;


    public void LoadScene()
    {
        StartCoroutine(LoadSceneAsyncRoutine(sceneToLoad));
    }

    private IEnumerator LoadSceneAsyncRoutine(string sceneName)
    {
        if (loadingScreenCanvas != null)
        {
            loadingScreenCanvas.SetActive(true);
        }

        yield return new WaitForSeconds(1f);

        Application.backgroundLoadingPriority = ThreadPriority.Low;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;


        while (asyncLoad.progress < 0.9f)
        {
            float progress01 = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            if (progressText != null)
            {
                // Format text without string concatenation allocations
                progressText.SetText("Loading... {0}%", Mathf.RoundToInt(progress01 * 100f));
            }

            yield return null;
        }

        if (progressText != null)
        {
            progressText.SetText("Loading... 100%");
        }
        yield return null;
        asyncLoad.allowSceneActivation = true;
    }
}