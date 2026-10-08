using UnityEngine;

[CreateAssetMenu(fileName = "HeroEquipment", menuName = "Scriptable Objects/Hero Equipment")]

public class HeroEquipmentObject : ScriptableObject //this class only for visual equipment!
{
    public enum EquipmentSlot
    {
        Helmet,
        MainHand,
        OffHand
        //Relic
        //Pet
        //...tabard?
    }
    [Header("Equipment Info")]
    public string equipmentName;
    public EquipmentSlot equipmentSlot;
    public Sprite equipmentIcon;

    [Header("Visuals")]
    public GameObject equipmentPrefab;

    [Header("Equipment Stats")]
    public  float healthBonus;
    public float staminaBonus;
    public float staminaRegenBonus;

    public float impactDamageBonus;
    public float weaponDamageBonus;

    public float maxSpeedBonus;
    public float bounceBonus;
    public float frictionBonus;




}
