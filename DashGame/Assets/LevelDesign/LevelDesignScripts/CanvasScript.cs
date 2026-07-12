using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] Image healthBar;
    [SerializeField] TextMeshProUGUI staminaText;
    [SerializeField] Image staminaBar;
    [SerializeField] TextMeshProUGUI keyText;
    [SerializeField] TextMeshProUGUI scoreText;
    float drawHealth;
    float drawStamina;
    float drawKey;
    float drawScore;
    float t = 0.1f;

    Coroutine countRoutine;

    private void Start()
    {
        drawHealth=LevelManager.inst.health;
        healthText.text ="Health: "+drawHealth.ToString("F0")+"/"+LevelManager.inst.maxHealth.ToString("F0");
        drawStamina=LevelManager.inst.stamina;
        staminaText.text ="Stamina: "+drawStamina.ToString("F0")+"/"+LevelManager.inst.maxStamina.ToString("F0");
        drawKey = LevelManager.inst.keys;
        keyText.text = "Keys: "+LevelManager.inst.keys.ToString("F0");
        drawScore=LevelManager.inst.score;
        scoreText.text = "Score: "+drawScore.ToString("F0");
    }

    public void UpdateHealth()
    {
        drawHealth = LevelManager.inst.health;
        healthText.text = "Health: " + drawHealth.ToString("F0") + "/" + LevelManager.inst.maxHealth.ToString("F0");
        float hp=LevelManager.inst.health/LevelManager.inst.maxHealth;
        healthBar.transform.localScale = new Vector3(hp,1,1);
    }

    public void UpdateStamina()
    {
        drawStamina = LevelManager.inst.stamina;
        staminaText.text = "Stamina: " + drawStamina.ToString("F0") + "/" + LevelManager.inst.maxStamina.ToString("F0");
        float sp=LevelManager.inst.stamina/LevelManager.inst.maxStamina;
        staminaBar.transform.localScale=new Vector3(sp,1,1);
    }
    public void UpdateKeys()
    {
        keyText.text = "Keys: "+ LevelManager.inst.keys.ToString("F0");
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
