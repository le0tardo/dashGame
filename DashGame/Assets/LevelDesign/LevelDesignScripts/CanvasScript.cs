using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{
    [Header("Fields")]
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] Image healthBar;
    [SerializeField] Image healthTrailBar;
    [SerializeField] Animator heartAnim;
    [SerializeField] TextMeshProUGUI staminaText;
    [SerializeField] Image staminaBar;
    [SerializeField] TextMeshProUGUI keyText;
    [SerializeField] TextMeshProUGUI xpText;
    [SerializeField] Image xpBar;
    [SerializeField] TextMeshProUGUI lvlText;
    float drawHealth;
    float drawStamina;
    float drawKey;
    float drawXp;
    float t = 0.1f;

    //trails
    float trailDelay = 0.25f;
    float trailDuration = 0.5f;

    Coroutine countRoutine;
    Coroutine trailRoutine;

    private void Start()
    {
        drawHealth=LevelManager.inst.health;
        healthText.text ="Health: "+drawHealth.ToString("F0")+"/"+LevelManager.inst.maxHealth.ToString("F0");
        drawStamina=LevelManager.inst.stamina;
        staminaText.text ="Stamina: "+drawStamina.ToString("F0")+"/"+LevelManager.inst.maxStamina.ToString("F0");
        drawKey = LevelManager.inst.keys;
        keyText.text = "Keys: "+LevelManager.inst.keys.ToString("F0");
        drawXp=LevelManager.inst.xp;
        xpText.text = "XP: "+drawXp.ToString("F0");

        UpdateHealth();
        UpdateStamina();
        UpdateXp();
        UpdateLevel();
    }

    public void UpdateHealth()
    {
        float currentHP = LevelManager.inst.health;
        float maxHP = LevelManager.inst.maxHealth;
        float targetFill = Mathf.Clamp01(currentHP / maxHP);

        healthText.text = "Health: " + currentHP.ToString("F0") + "/" + maxHP.ToString("F0");;
        healthBar.fillAmount = targetFill;
        trailRoutine = StartCoroutine(AnimateTrail(targetFill));
        if (heartAnim != null) heartAnim.SetTrigger("wobble");
    }

    public void UpdateStamina()
    {
        float currentStamina= LevelManager.inst.stamina;
        float maxStamina = LevelManager.inst.maxStamina;
        float staminaPercent=currentStamina/maxStamina;

        staminaText.text = "Stamina: " + currentStamina.ToString("F0") + "/" + maxStamina.ToString("F0");
        staminaBar.fillAmount= staminaPercent;
    }
    public void UpdateKeys()
    {
        keyText.text = "Keys: "+ LevelManager.inst.keys.ToString("F0");
    }

    public void UpdateXp()
    {
        float currentXp=LevelManager.inst.xp;
        float maxXp=LevelManager.inst.maxXp;
        float xpPercent=currentXp/maxXp;
        //xpText.text="XP: "+drawXp.ToString("F0")+"/"+LevelManager.inst.maxXp.ToString("F0");
        float xp = LevelManager.inst.xp / LevelManager.inst.maxXp;
        xpBar.fillAmount = xpPercent;
    }

    public void UpdateLevel() //keep separate if i wanna add some flare here later..
    {
        //lvlText.text = "lvl: " + LevelManager.inst.level.ToString();
        lvlText.text = LevelManager.inst.level.ToString();
    }


    private IEnumerator AnimateTrail(float targetFill)
    {
        // Brief pause so the player sees how much damage was just dealt
        yield return new WaitForSeconds(trailDelay);

        float initialTrailFill = healthTrailBar.fillAmount;
        float elapsedTime = 0f;

        while (elapsedTime < trailDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / trailDuration;

            // SmoothStep provides a nice mechanical ease-out
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            healthTrailBar.fillAmount = Mathf.Lerp(initialTrailFill, targetFill, smoothT);
            yield return null;
        }

        healthTrailBar.fillAmount = targetFill;
    }
}
