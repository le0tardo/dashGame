using System.Collections;
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

    Coroutine countRoutine = null;
    float countDuration = 0.5f;
    [SerializeField] AudioClip goldCountSound;

    [Header("Unlock feedback")]
    [SerializeField] GameObject unlockFX;
    Animator unlockFxAnimator;
    [SerializeField] AudioClip unlockSound;
    [SerializeField] AudioClip tooPoorSound;


    [SerializeField] SessionData sessionData;

    private void Awake()
    {
        inst = this;

        gold = sessionData.gold;
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

        unlockFxAnimator=unlockFX.GetComponent<Animator>();
    }

    public void LevelUp()
    {
        //visuals first?
        SpendGoldVisuals(gold,gold-levelUpCost);

        gold-=levelUpCost;

        levelUpCost = Mathf.RoundToInt(baseLevelUpCost *e);

        LobbyStatManager.inst.LevelUp();

        //draw new values
       // goldAmount.text = gold.ToString(); //corotuined instead
        goldCost.text = levelUpCost.ToString();

        levelUpButton.interactable = gold >= levelUpCost;

        if (levelUpButton.interactable) levelUpIcon.sprite = plus[0];
        else levelUpIcon.sprite = plus[1];

    }

    public void SpendGoldVisuals(int goldBefore, int goldAfter)
    {
        AudioManager.inst.PlayCustomSound(goldCountSound, 1f);

        levelUpButton.interactable = gold >= levelUpCost;

        if (countRoutine != null)
        {
            StopCoroutine(countRoutine);
        }
        countRoutine = StartCoroutine(CountRoutine(goldBefore, goldAfter));
    }

    IEnumerator CountRoutine(int oldValue, int newValue)
    {
        float elapsedTime = 0f;

        while (elapsedTime < countDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;

            float progress = Mathf.Clamp01(elapsedTime / countDuration);
            float currentValue=Mathf.Lerp(oldValue, newValue, progress);
            currentValue=Mathf.RoundToInt(currentValue);
            goldAmount.SetText("{0}",currentValue);
            yield return null;
        }

        goldAmount.SetText("{0}", newValue);
        countRoutine = null;
    }

    public void UnlockFX(Vector3 position)
    {
        unlockFX.transform.position= position;
        unlockFxAnimator.Play("unlock");
        AudioManager.inst.PlayCustomSound(unlockSound, 1f);
    }

    public void TooPoor()
    {
        AudioManager.inst.PlayCustomSound(tooPoorSound, 0.85f);
    }

}
