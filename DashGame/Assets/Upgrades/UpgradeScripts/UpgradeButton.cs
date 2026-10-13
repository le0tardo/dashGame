using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] UpgradeObject upgrade;

    [SerializeField] TextMeshProUGUI upgradeTitle;
    [SerializeField] Image upgradeIcon;

    [SerializeField] TextMeshProUGUI upgradeAmount;

    private void Start() //debug test
    {
        SetUpgrade(upgrade);
    }
    public void SetUpgrade(UpgradeObject upgradeObj)
    {
        upgrade = upgradeObj;

        upgradeTitle.text=upgrade.upgradeTitle;
        upgradeIcon.sprite=upgrade.upgradeIcon;
        upgradeAmount.text = "+" + upgrade.upgradeAmount.ToString();

    }

}
