using TMPro;
using UnityEngine;

public class LobbyStatsUI : MonoBehaviour
{
    public static LobbyStatsUI inst;

    [SerializeField] TextMeshProUGUI levelString;
    [SerializeField] TextMeshProUGUI health;
    [SerializeField] TextMeshProUGUI stamina;
    [SerializeField] TextMeshProUGUI weapon;
    [SerializeField] TextMeshProUGUI impact;
    [SerializeField] TextMeshProUGUI defence;
    [SerializeField] TextMeshProUGUI speed;
    [SerializeField] TextMeshProUGUI drag;
    [SerializeField] TextMeshProUGUI bounce;


    private void Awake()
    {
        inst = this;
    }

    private void Start()
    {
        DrawStats();
    }
    public void DrawStats()
    {
        var stats = LobbyStatManager.inst;
        levelString.text= stats.currentHero.heroLevel.ToString();
        health.text = stats.heroHealth.ToString();
        stamina.text=stats.heroStamina.ToString();
        weapon.text=stats.heroWeaponDamage.ToString();
        impact.text=stats.heroImpactDamage.ToString();
        defence.text=stats.heroDefence.ToString();
        speed.text=stats.heroMaxSpeed.ToString();
        drag.text=stats.heroDrag.ToString();
        bounce.text=(stats.heroBounciness*100).ToString();
    }
}
