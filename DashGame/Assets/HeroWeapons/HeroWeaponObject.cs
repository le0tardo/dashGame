using UnityEngine;

public enum ElementalType
{
    None,
    Fire,
    Ice,
    Poison,
    Light
}

[CreateAssetMenu(fileName = "HeroWeapon", menuName = "Scriptable Objects/Hero Weapon")]

public class HeroWeaponObject : ScriptableObject
{
    public string weaponName;
    public Sprite weaponIcon;
    public GameObject weaponModel;
    public float weaponPhysicalDamage;
    public float weaponElementalDamage;
    public ElementalType weaponElement;
    public float weaponCritChance;
    public float weaponCritRate;
}
