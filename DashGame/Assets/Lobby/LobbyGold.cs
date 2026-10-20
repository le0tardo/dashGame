using TMPro;
using UnityEditor;
using UnityEngine;

public class LobbyGold : MonoBehaviour
{
    public static LobbyGold inst;
    [SerializeField] public int gold;
    [SerializeField] TextMeshProUGUI goldAmount;
    [SerializeField] TextMeshProUGUI goldCost;

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
    }

    public void LevelUp()
    {
        gold-=levelUpCost;

        levelUpCost = Mathf.RoundToInt(baseLevelUpCost *e);

        LobbyStatManager.inst.LevelUp();



        //draw new values
        goldAmount.text = gold.ToString();
        goldCost.text = levelUpCost.ToString();
    }
}
