using UnityEngine;

[CreateAssetMenu(fileName = "HeroStats", menuName = "Scriptable Objects/Hero Stats")]
public class HeroStatsObject : ScriptableObject
{
    [Header("Info")]
    public string heroName;
    public Sprite heroIcon;
    public int heroLevel;
    // add mesh and material here i guess?

    [Header("Stats")]
    public float heroHealth;
    public float heroStamina;
    public float heroStaminaRegenRate;
    public float heroImpactDamage;
    public float heroWeaponDamage;
    public float heroMaxSpeed;

    [Header("Equipment")]
    public HeroWeaponObject heroWeapon;
    public PetObject heroPet;

}
