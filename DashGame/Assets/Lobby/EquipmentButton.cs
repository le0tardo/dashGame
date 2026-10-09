using UnityEngine;
using UnityEngine.UI;

public class EquipmentButton : MonoBehaviour
{
    [SerializeField] HeroEquipmentObject equipment;
    [SerializeField] Image icon;

    private void Start()
    {
        icon.sprite = equipment.equipmentIcon;
    }

    public void SetEquipment()
    {
        switch (equipment.equipmentSlot)
        {
            case HeroEquipmentObject.EquipmentSlot.Helmet:
                LobbyEquipmentManager.inst.currentHelmet = equipment;
            break;

            case HeroEquipmentObject.EquipmentSlot.MainHand:
                LobbyEquipmentManager.inst.currentMainHand = equipment;
            break;

            case HeroEquipmentObject.EquipmentSlot.OffHand:
                LobbyEquipmentManager.inst.currentOffHand = equipment;
            break;

            default:
                print("invalid equipment slot");
            break;
        }
    }
}
