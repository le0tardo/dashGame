using TMPro;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] CameraShake cam;

    [Header("Stats")]
    [SerializeField] TextMeshProUGUI healthStat;
    [SerializeField] TextMeshProUGUI staminaStat;
    [SerializeField] TextMeshProUGUI weaponStat;
    [SerializeField] TextMeshProUGUI impactStat;
    [SerializeField] TextMeshProUGUI defenceStat;
    [SerializeField] TextMeshProUGUI regenStat;
    [SerializeField] TextMeshProUGUI speedStat;
    [SerializeField] TextMeshProUGUI bounceStat;
    [SerializeField] TextMeshProUGUI dragStat;
    private void OnEnable()
    {
        print("Pause menu enabled");
        LevelManager.inst.timeScale = 0;
        if(cam!=null)cam.StopAllCoroutines();
        SetStats();
    }

    private void OnDisable()
    {
        print("Pause menu disabled");
        LevelManager.inst.timeScale = 1;
    }

    public void ResumeButton()
    {
        this.gameObject.SetActive(false);
    }

    public void RestartButton()
    {
        LevelManager.inst.ResetLevel();
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    void SetStats()
    {
        var hero = LevelManager.inst.heroStats;
        healthStat.text= hero.heroHealth.ToString();
        staminaStat.text= hero.heroStamina.ToString();
        weaponStat.text=hero.heroWeaponDamage.ToString();
        impactStat.text=hero.heroImpactDamage.ToString();
        defenceStat.text=hero.heroDefence.ToString();
        regenStat.text=hero.heroStaminaRegenRate.ToString();
        speedStat.text=hero.heroMaxSpeed.ToString();
        bounceStat.text=(hero.heroBounce*100).ToString();
        dragStat.text=hero.heroDrag.ToString();
    }
}
