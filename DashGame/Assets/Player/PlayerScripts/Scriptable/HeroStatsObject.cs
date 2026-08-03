using UnityEngine;

[CreateAssetMenu(fileName = "HeroStats", menuName = "Scriptable Objects/Hero Stats")]
public class HeroStatsObject : ScriptableObject
{
    [Header("Stats")]
    public string heroName;
    public int heroLevel;
    public float heroHealth;
    public float heroStamina;
    public float heroRegenRate;
    public float heroDamage;
    public float heroSpeed;

    [Header("Equipment")]
    public HeroWeaponObject heroWeapon;
}
