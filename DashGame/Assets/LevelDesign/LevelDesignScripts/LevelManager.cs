using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    public static LevelManager inst { get; private set; }

    [Header("Level Stats")]
    [SerializeField] public float health;
    [SerializeField] public float maxHealth;
    [SerializeField] public float stamina;
    [SerializeField] public float maxStamina;
    [SerializeField] public float staminaRegenTime=1f;
    [SerializeField] private float staminaRegenRate = 10f;
    [SerializeField] public float staminaRegenDelay=1f;

    [SerializeField] public float keys;

    [SerializeField] float timeScale=1f;
    [SerializeField] public float score=0f;

    [SerializeField] CanvasScript canvas;


    private void Awake()
    {
        inst = this;
        maxHealth = health;
        GameObject bulletPool = GameObject.Find("BulletPool");
        if (bulletPool == null) { new GameObject("BulletPool");}
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }

        Time.timeScale = timeScale;

        if (stamina < maxStamina)
        {
            FillStamina();
        }
    }

    public void ChangeHealth(int _health) //for both + and - health
    {
        health += _health;
        canvas.UpdateHealth();
    }

    public void UseStamina(float amount)
    {
        stamina = Mathf.Clamp(stamina - amount, 0f, maxStamina);
        print("used "+amount +" stamina");
        canvas.UpdateStamina();

        staminaRegenTime = staminaRegenDelay;//pause before regenig
    }
    void FillStamina()
    {
        if (staminaRegenTime > 0f)
        {
            staminaRegenTime-= Time.deltaTime;//unsclaed time for consistant regen on aim
            return;
        }
        if (stamina < maxStamina)
        {
            stamina += staminaRegenRate * Time.unscaledDeltaTime;

            if (stamina > maxStamina)
            {
                stamina = maxStamina;
            }
        }
        canvas.UpdateStamina();
    }
    public void AddScore(float scr)
    {
        score += Mathf.RoundToInt(scr);
        canvas.UpdateScoreText(score);

    }

    public void AddKey()
    {
        keys++;
        canvas.UpdateKeys();
    }
    public void SlowDownTime()
    {
        StopAllCoroutines();
        StartCoroutine(SlowDown());
    }

    public void ResetTime()
    {
        StopAllCoroutines();
        timeScale = 1f;
    }
    private IEnumerator SlowDown()
    {
        float startScale = timeScale;
        float targetScale = 0.25f;
        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / duration;
            timeScale = Mathf.Lerp(startScale, targetScale, normalizedTime);
            yield return null;
        }

        timeScale = targetScale; //snap
    }
}
