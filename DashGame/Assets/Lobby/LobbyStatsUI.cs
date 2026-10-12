using TMPro;
using UnityEngine;

public class LobbyStatsUI : MonoBehaviour
{
    public static LobbyStatsUI inst;

    [SerializeField] TextMeshProUGUI health;
    [SerializeField] TextMeshProUGUI stamina;
    [SerializeField] TextMeshProUGUI weapon;
    [SerializeField] TextMeshProUGUI impact;
    [SerializeField] TextMeshProUGUI speed;

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
        health.text = stats.heroHealth.ToString();
        stamina.text=stats.heroStamina.ToString();
        weapon.text=stats.heroWeaponDamage.ToString();
        impact.text=stats.heroImpactDamage.ToString();
        speed.text=stats.heroMaxSpeed.ToString();
    }
}
