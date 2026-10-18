using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] public UpgradeObject upgrade;

    [SerializeField] TextMeshProUGUI upgradeTitle;
    [SerializeField] Image upgradeIcon;

    [SerializeField] TextMeshProUGUI upgradeAmount;

    [SerializeField] ChosenUpgradeFX choosenUpgrade;
    private void Start() //debug test
    {
        SetUpgrade(upgrade);
    }
    public void SetUpgrade(UpgradeObject upgradeObj)
    {
        upgrade = upgradeObj;

        string upgradeTypeString = GetStatName(upgradeObj.upgradeStat);


        upgradeTitle.text=upgrade.upgradeTitle;
        upgradeIcon.sprite=upgrade.upgradeIcon;
        upgradeAmount.text = "+" + upgrade.upgradeAmount.ToString() + " "+upgradeTypeString;

    }

    public void ApplyUpgrade()
    {
        if (upgrade == null || LevelManager.inst == null || LevelManager.inst.heroStats == null)
        {
            return;
        }

        var stats = LevelManager.inst.heroStats;

        switch (upgrade.upgradeStat)
        {
            case UpgradeObject.UpgradeStat.HealthBonus:
                stats.heroHealth += upgrade.upgradeAmount;
                break;

            case UpgradeObject.UpgradeStat.StaminaBonus:
                stats.heroStamina += upgrade.upgradeAmount;
                break;

            case UpgradeObject.UpgradeStat.StaminaRegenRate:
                stats.heroStaminaRegenRate += upgrade.upgradeAmount;
                break;

            case UpgradeObject.UpgradeStat.StaminaRegenSpeed:
                stats.heroStaminaRegenSpeed += upgrade.upgradeAmount;
                break;

            case UpgradeObject.UpgradeStat.ImpactDamageBonus:
                stats.heroImpactDamage += upgrade.upgradeAmount;
                break;

            case UpgradeObject.UpgradeStat.WeaponDamageBonus:
                stats.heroWeaponDamage += upgrade.upgradeAmount;
                break;

            case UpgradeObject.UpgradeStat.DefenceBonus:
                stats.heroDefence += upgrade.upgradeAmount;
                break;

            case UpgradeObject.UpgradeStat.MaxSpeedBonus:
                stats.heroMaxSpeed += upgrade.upgradeAmount;
                break;

            case UpgradeObject.UpgradeStat.DragBonus:
                stats.heroDrag += upgrade.upgradeAmount;
                break;

            case UpgradeObject.UpgradeStat.BounceBonus:
                stats.heroBounce += upgrade.upgradeAmount;
                break;

            default:
                Debug.LogWarning($"Unhandled UpgradeStat: {upgrade.upgradeStat}");
                break;
        }

        choosenUpgrade.ChoosenUpgradeFX(transform.position,upgrade);
    }

    public static string GetStatName(UpgradeObject.UpgradeStat stat)
    {
        return stat switch
        {
            UpgradeObject.UpgradeStat.HealthBonus => "Health",
            UpgradeObject.UpgradeStat.StaminaBonus => "Stamina",
            UpgradeObject.UpgradeStat.StaminaRegenRate => "Stamina Regen",
            UpgradeObject.UpgradeStat.StaminaRegenSpeed => "dont use this stat",
            UpgradeObject.UpgradeStat.ImpactDamageBonus => "Impact Damage",
            UpgradeObject.UpgradeStat.WeaponDamageBonus => "Weapon Damage",
            UpgradeObject.UpgradeStat.DefenceBonus => "Defence",
            UpgradeObject.UpgradeStat.MaxSpeedBonus => "Max Speed",
            UpgradeObject.UpgradeStat.DragBonus => "Drag",
            UpgradeObject.UpgradeStat.BounceBonus => "Bounce",
            _ => string.Empty
        };
    }

}
