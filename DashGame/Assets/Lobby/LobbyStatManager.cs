using System.Diagnostics;
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
    public float heroDrag;
    public float heroBounciness;

    [Header("Elemental")]
    public float heroFirePower;
    public float heroFrostPower;
    public float heroShockPower;

    //default stats
    float defaultHealth;
    float defaultDefence;
    float defaultImpactDamage;
    public float defaultWeaponDamage;
    public float defaultStamina;
    public float defaultMaxSpeed;
    float defaultFriction;

    [Header("Session Data")]
    [SerializeField] SessionData sessionData;

    [Header("Level up Feedback")]
    [SerializeField] ParticleSystem[] particles;

    private void Awake()
    {
        inst = this;
    }

    private void Start()
    {
        defaultHealth = currentHero.heroHealth;
        defaultDefence = currentHero.heroDefence;
        defaultImpactDamage = currentHero.heroImpactDamage;
        defaultWeaponDamage= currentHero.heroWeaponDamage;
        defaultStamina = currentHero.heroStamina;
        defaultMaxSpeed = currentHero.heroMaxSpeed;
        defaultFriction=currentHero.heroDrag;

        UpdateStats();
    }

    public void UpdateStats()
    {
        level = currentHero.heroLevel;
        levelMultiplier = currentHero.heroLevel * 12;

        heroHealth = currentHero.heroHealth + levelMultiplier;
        heroDefence = currentHero.heroDefence + levelMultiplier;
        heroImpactDamage = currentHero.heroImpactDamage + levelMultiplier;
        heroWeaponDamage = currentHero.heroWeaponDamage + levelMultiplier;

        heroStamina = currentHero.heroStamina + levelMultiplier;
        heroStaminaRecoveryRate = currentHero.heroStaminaRegenRate + level;
        heroStaminaRecoverySpeed = currentHero.heroStaminaRegenSpeed + level;

        heroMaxSpeed = currentHero.heroMaxSpeed + levelMultiplier;
        heroDrag = currentHero.heroDrag; //idk how i wanna tweak this... runtime only?
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
        currentHero.heroDrag -= oldEquipment.frictionBonus;
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
        currentHero.heroDrag += equipment.frictionBonus;

        UpdateStats();
        LobbyStatsUI.inst.DrawStats();

    }

    public void LevelUp()
    {
        currentHero.heroLevel++;

        UpdateStats();

        foreach(ParticleSystem particle in particles)
        {
            particle.Play();
        }
    }

    public void PopulateSessionData()
    {
        sessionData.hero = currentHero;

        sessionData.health = currentHero.heroHealth;
        sessionData.stamina = currentHero.heroStamina;
        sessionData.impactDamage = currentHero.heroImpactDamage;
        sessionData.weaponDamage = currentHero.heroWeaponDamage;
        sessionData.defence = currentHero.heroDefence;
        sessionData.maxSpeed = currentHero.heroMaxSpeed;
        sessionData.drag = currentHero.heroDrag;
        sessionData.bounce = currentHero.heroBounce;

        sessionData.helmet = LobbyEquipmentManager.inst.currentHelmet;
        sessionData.mainHand = LobbyEquipmentManager.inst.currentMainHand;
        sessionData.offHand = LobbyEquipmentManager.inst.currentOffHand;
    }

    private void OnApplicationQuit()
    {
#if UNITY_EDITOR
        //only do thos if in unity editor:
        currentHero.heroLevel = 1;
        currentHero.heroHealth = defaultHealth;
        currentHero.heroDefence = defaultDefence;
        currentHero.heroImpactDamage = defaultImpactDamage;
        currentHero.heroWeaponDamage=defaultWeaponDamage;
        currentHero.heroStamina = defaultStamina;
        currentHero.heroMaxSpeed = defaultMaxSpeed;
        currentHero.heroDrag = defaultFriction;

        //also clear session data here
        sessionData.hero = null;
        //TODO add level
        sessionData.health = 0; sessionData.stamina = 0;
        sessionData.impactDamage = 0; sessionData.weaponDamage = 0; sessionData.defence = 0;
        sessionData.maxSpeed = 0; sessionData.drag = 0; sessionData.bounce = 0;
        sessionData.helmet = null;sessionData.mainHand = null;sessionData.offHand = null;

#endif
    }
}
