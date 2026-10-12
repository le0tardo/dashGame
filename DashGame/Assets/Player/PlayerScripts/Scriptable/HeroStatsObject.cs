using UnityEngine;

[CreateAssetMenu(fileName = "HeroStats", menuName = "Scriptable Objects/Hero Stats")]
public class HeroStatsObject : ScriptableObject
{
    [Header("Hero Info")]
    public string heroName;
    public Sprite heroIcon;
    public int heroLevel;

    [Header("Combat Stats")]
    public float heroHealth;
    public float heroImpactDamage;
    public float heroWeaponDamage;
    public float heroDefence;

    [Header("Movement stats")]
    public float heroStamina;
    public float heroStaminaRegenRate;
    public float heroStaminaRegenSpeed;
    public float heroMaxSpeed;
    public float heroDeceleration;
    public float heroBounce;

    [Header("Elemental")]
    public float fireDamage;
    public float frostDamage;
    public float shockDamage;


}
