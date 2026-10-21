using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyGold : MonoBehaviour
{
    public static LobbyGold inst;
    [SerializeField] public int gold;
    [SerializeField] TextMeshProUGUI goldAmount;
    [SerializeField] TextMeshProUGUI goldCost;
    [SerializeField] Button levelUpButton;
    [SerializeField] Image levelUpIcon;
    [SerializeField] Sprite[] plus;

    public int currentLevel;
    public int levelUpCost;
    int baseLevelUpCost=100;
    float e = 2.7f;

    private void Awake()
    {
        inst = this;
    }

    private void Start()
    {
        currentLevel = LobbyStatManager.inst.currentHero.heroLevel;
        levelUpCost = baseLevelUpCost * currentLevel;   

        goldAmount.text = gold.ToString();
        goldCost.text = levelUpCost.ToString();

        levelUpButton.interactable = gold >= levelUpCost;

        if (levelUpButton.interactable) levelUpIcon.sprite = plus[0];
        else levelUpIcon.sprite = plus[1];
    }

    public void LevelUp()
    {
        gold-=levelUpCost;

        levelUpCost = Mathf.RoundToInt(baseLevelUpCost *e);

        LobbyStatManager.inst.LevelUp();

        //draw new values
        goldAmount.text = gold.ToString();
        goldCost.text = levelUpCost.ToString();

        levelUpButton.interactable = gold >= levelUpCost;

        if (levelUpButton.interactable) levelUpIcon.sprite = plus[0];
        else levelUpIcon.sprite = plus[1];

    }
}
