using UnityEngine;

public enum DamageType
{
    Physical,
    Fire,
    Ice,
    Poison,
    Light
}

[CreateAssetMenu(fileName = "HeroWeapon", menuName = "Scriptable Objects/Hero Weapon")]

public class HeroWeaponObject : ScriptableObject
{
    public string weaponName;
    public DamageType weaponDamageType;
    public float weaponDamage;
    public float weaponCritChance;
    public float weaponCritRate;
}
