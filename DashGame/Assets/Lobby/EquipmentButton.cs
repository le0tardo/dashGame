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
    }
}
