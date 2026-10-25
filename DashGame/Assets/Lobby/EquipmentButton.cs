using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EquipmentButton : MonoBehaviour
{
    [SerializeField] HeroEquipmentObject equipment;
    [SerializeField] Image icon;
    [SerializeField] RectTransform equippedMarker;

    [Header("unlock")]
    [SerializeField] bool unlocked = false;
    [SerializeField] GameObject lockedGfx;
    [SerializeField] int goldCost;
    TextMeshProUGUI costText;

    [Header("Too Poor")]
    float flashDuration = 0.33f;
    Coroutine flashRoutine = null;

    private void Start()
    {
        icon.sprite = equipment.equipmentIcon;

        costText=lockedGfx.GetComponentInChildren<TextMeshProUGUI>();
        costText.text=goldCost.ToString();

        if (unlocked)
        {
            lockedGfx.SetActive(false);
        }
    }

    public void UnlockOrEquip()
    {
        if (!unlocked)
        {
            if (LobbyGold.inst.gold >= goldCost)
            {
                Unlock();
            }
            else
            {
                TooPoor();
            }
        }
        else
        {
            SetEquipment();
        }
    }
    public void Unlock()
    {
        if (!unlocked) { unlocked = true; }
        lockedGfx.SetActive(false);

        LobbyGold.inst.gold -= goldCost;
        LobbyGold.inst.SpendGoldVisuals(LobbyGold.inst.gold+goldCost,LobbyGold.inst.gold);
        LobbyGold.inst.UnlockFX(transform.position);

    }

    public void SetEquipment()
    {
        LobbyEquipmentManager.inst.Equip(equipment);

        equippedMarker.transform.position=transform.position;
    }

    public void TooPoor()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }
        flashRoutine = StartCoroutine(FlashRoutine(Color.white, Color.red));

        LobbyGold.inst.TooPoor();
    }

    IEnumerator FlashRoutine(Color defaultColor, Color flashColor)
    {
        float elapsed = 0;

        while (elapsed <flashDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float progress = Mathf.Clamp01(elapsed / flashDuration);

            float pingPongProgress = Mathf.PingPong(progress * 4f, 1f);

            costText.color = Color.Lerp(defaultColor, flashColor, pingPongProgress);

            yield return null;
        }

        costText.color = defaultColor;
        flashRoutine= null; 
    }
}
