using UnityEngine;

public class LobbyStatManager : MonoBehaviour
{
    public static LobbyStatManager inst;

    public HeroStatsObject currentHero;

    float level;
    float levelMultiplier;

    [Header("Combat")]
    public float heroHealth;
    public float heroDefence;
    public float heroImpactDamage;
    public float heroWeaponDamage;

    [Header("Stamina")]
    public float heroStamina;
    public float heroStaminaRecoveryRate;
    public float heroStaminaRecoverySpeed;

    [Header("Movement")]
    public float heroMaxSpeed;
    public float heroFriction;
    public float heroBounciness;

    [Header("Elemental")]
    public float heroFirePower;
    public float heroFrostPower;
    public float heroShockPower;


    private void Awake()
    {
        inst = this;
    }

    private void Start()
    {
        UpdateStats();
    }

    public void UpdateStats()
    {
        level = currentHero.heroLevel;
        levelMultiplier = currentHero.heroLevel * 10;

        heroHealth = currentHero.heroHealth + levelMultiplier;
        heroDefence = currentHero.heroDefence + levelMultiplier;
        heroImpactDamage = currentHero.heroImpactDamage + levelMultiplier;
        heroWeaponDamage = currentHero.heroWeaponDamage + levelMultiplier;

        heroStamina = currentHero.heroStamina + levelMultiplier;
        heroStaminaRecoveryRate = currentHero.heroStaminaRegenRate + level;
        heroStaminaRecoverySpeed = currentHero.heroStaminaRegenSpeed + level;

        heroMaxSpeed = currentHero.heroMaxSpeed + levelMultiplier;
        heroFriction = currentHero.heroDeceleration; //idk how i wanna tweak this... runtime only?
        heroBounciness = currentHero.heroBounce + (level / 10);

        LobbyStatsUI.inst.DrawStats();
    }

    public void UnequipItem(HeroEquipmentObject oldEquipment)
    {
        currentHero.heroHealth -= oldEquipment.healthBonus;
        currentHero.heroStamina -= oldEquipment.staminaBonus;
        currentHero.heroStaminaRegenRate -= oldEquipment.staminaRegenBonus;
        currentHero.heroImpactDamage -= oldEquipment.impactDamageBonus;
        currentHero.heroWeaponDamage -= oldEquipment.weaponDamageBonus;
        currentHero.heroDefence -= oldEquipment.defenceBonus;
        currentHero.heroMaxSpeed -= oldEquipment.maxSpeedBonus;
        currentHero.heroBounce -= oldEquipment.bounceBonus;
        currentHero.heroDeceleration -= oldEquipment.frictionBonus;
    }
    public void EquipItem(HeroEquipmentObject equipment)
    {
        currentHero.heroHealth += equipment.healthBonus;
        currentHero.heroStamina += equipment.staminaBonus;
        currentHero.heroStaminaRegenRate += equipment.staminaRegenBonus;
        currentHero.heroImpactDamage += equipment.impactDamageBonus;
        currentHero.heroWeaponDamage += equipment.weaponDamageBonus;
        currentHero.heroDefence += equipment.defenceBonus;
        currentHero.heroMaxSpeed += equipment.maxSpeedBonus;
        currentHero.heroBounce += equipment.bounceBonus;
        currentHero.heroDeceleration += equipment.frictionBonus;

        UpdateStats();
        LobbyStatsUI.inst.DrawStats();

    }
}
