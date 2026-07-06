using System.Collections;
using TMPro;
using UnityEngine;

public class CanvasScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    float drawScore;
    float t = 0.1f;

    Coroutine countRoutine;

    private void Start()
    {
        drawScore=LevelManager.inst.score;
        scoreText.text = "Score: "+drawScore.ToString("F0");
    }
    public void UpdateScoreText(float scr)
    {
        if (countRoutine != null)
        {
            StopCoroutine(countRoutine);
        }
        countRoutine = StartCoroutine(CountScore(scr));
    }

    IEnumerator CountScore(float scoreToAdd)
    {
        float startScore = drawScore;
        float targetScore = drawScore + scoreToAdd;
        float elapsedTime = 0f;

        while (elapsedTime < t)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / t;
            float smoothTime = Mathf.SmoothStep(0f, 1f, normalizedTime);
            drawScore = Mathf.Lerp(startScore, targetScore, smoothTime);
            scoreText.text = "Score: "+drawScore.ToString("F0");
            yield return null; 
        }
        drawScore = targetScore;
        scoreText.text = "Score: "+drawScore.ToString("F0");
        countRoutine = null;
    }
}
