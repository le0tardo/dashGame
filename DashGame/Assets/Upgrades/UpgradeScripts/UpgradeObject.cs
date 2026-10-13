using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Scriptable Objects/Upgrade")]
public class UpgradeObject : ScriptableObject
{
    public enum UpgradeStat
    {
        HealthBonus,
        StaminaBonus,
        StaminaRegenRate,
        StaminaRegenSpeed,
        ImpactDamageBonus,
        WeaponDamageBonus,
        DefenceBonus,
        MaxSpeedBonus,
        DragBonus,
        BounceBonus
    }

    public string upgradeTitle;
    public Sprite upgradeIcon;
    public UpgradeStat upgradeStat;
    public float upgradeAmount;

}
