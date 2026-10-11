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
        LobbyEquipmentManager.inst.Equip(equipment);
        /*
        switch (equipment.equipmentSlot)
        {
            case HeroEquipmentObject.EquipmentSlot.Helmet:
                LobbyEquipmentManager.inst.Equip(equipment);

            break;

            case HeroEquipmentObject.EquipmentSlot.MainHand:
                LobbyEquipmentManager.inst.Equip(equipment);
            break;

            case HeroEquipmentObject.EquipmentSlot.OffHand:
                LobbyEquipmentManager.inst.currentOffHand = equipment;
            break;

            default:
                print("invalid equipment slot");
            break;
        }
        */
    }
}
