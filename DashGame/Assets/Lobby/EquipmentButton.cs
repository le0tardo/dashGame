using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
                //show some "NOT ENOUGH GOLD" gfx here...
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

        //make some GFX feedback here...

        LobbyGold.inst.gold -= goldCost;
        LobbyGold.inst.SpendGoldVisuals(LobbyGold.inst.gold+goldCost,LobbyGold.inst.gold);
        LobbyGold.inst.UnlockFX(transform.position);

    }

    public void SetEquipment()
    {
        LobbyEquipmentManager.inst.Equip(equipment);

        equippedMarker.transform.position=transform.position;
    }
}
