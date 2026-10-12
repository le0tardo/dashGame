using UnityEngine;
using UnityEngine.UI;

public class EquipmentButton : MonoBehaviour
{
    [SerializeField] HeroEquipmentObject equipment;
    [SerializeField] Image icon;

    [SerializeField] RectTransform equippedMarker;

    private void Start()
    {
        icon.sprite = equipment.equipmentIcon;
    }

    public void SetEquipment()
    {
        LobbyEquipmentManager.inst.Equip(equipment);

        equippedMarker.transform.position=transform.position;
    }
}
