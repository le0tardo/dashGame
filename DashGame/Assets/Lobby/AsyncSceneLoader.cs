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

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        asyncLoad.allowSceneActivation = false;

        float targetProgress = 0f;

        while (!asyncLoad.isDone)
        {
            // progress cap 90% in unity?
            targetProgress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            if (progressText != null)
            {
                progressText.text = "Loading... " + ((asyncLoad.progress / 0.9f) * 100).ToString();
            }

            if (asyncLoad.progress >= 0.9f)
            {
                //yield return new WaitForSeconds(0.2f);

                asyncLoad.allowSceneActivation = true;
            }
            yield return null; 
        }
    }
}