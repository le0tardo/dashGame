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
        var levelStats = LevelManager.inst;

        healthStat.text= levelStats.maxHealth.ToString();
        staminaStat.text= levelStats.maxStamina.ToString();
        weaponStat.text=levelStats.weaponDamage.ToString();
        impactStat.text=levelStats.impactDamage.ToString();
        defenceStat.text=levelStats.defence.ToString();
        regenStat.text= levelStats.staminaRegenRate.ToString();
        speedStat.text=levelStats.maxSpeed.ToString();
        bounceStat.text=(levelStats.bounce*100).ToString();
        dragStat.text=levelStats.drag.ToString();
    }
}
