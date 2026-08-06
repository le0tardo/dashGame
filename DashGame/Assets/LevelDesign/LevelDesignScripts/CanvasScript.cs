using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{
    [Header("Fields")]
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] Image healthBar;
    [SerializeField] TextMeshProUGUI staminaText;
    [SerializeField] Image staminaBar;
    [SerializeField] TextMeshProUGUI keyText;
    [SerializeField] TextMeshProUGUI xpText;
    [SerializeField] Image xpBar;
    float drawHealth;
    float drawStamina;
    float drawKey;
    float drawXp;
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
        drawXp=LevelManager.inst.xp;
        xpText.text = "XP: "+drawXp.ToString("F0");

        UpdateHealth();
        UpdateStamina();
        UpdateXp();
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

    public void UpdateXp()
    {
        drawXp=LevelManager.inst.xp;
        xpText.text="XP: "+drawXp.ToString("F0")+"/"+LevelManager.inst.maxXp.ToString("F0");
        float xp = LevelManager.inst.xp / LevelManager.inst.maxXp;
        xpBar.transform.localScale = new Vector3(xp,1,1);
    }


}
