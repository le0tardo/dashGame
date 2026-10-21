using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentButton : MonoBehaviour
{
    [SerializeField] HeroEquipmentObject equipment;
    [SerializeField] Image icon;

    [SerializeField] RectTransform equippedMarker;

    [Header("unlock")]
    [SerializeField] int goldCost;
    [SerializeField] bool unlocked=false;
    [SerializeField] TextMeshProUGUI costText;

    private void Start()
    {
        icon.sprite = equipment.equipmentIcon;
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
        LobbyGold.inst.gold -= goldCost;

    }

    public void SetEquipment()
    {
        LobbyEquipmentManager.inst.Equip(equipment);

        equippedMarker.transform.position=transform.position;
    }
}
