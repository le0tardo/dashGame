using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlots : MonoBehaviour
{
    [SerializeField] Image helmet;
    [SerializeField] Image mainHand;
    [SerializeField] Image offHand;
    [SerializeField] Image pet;

    [SerializeField] Sprite emptySprite;

    private void Start()
    {
        SetSlotIcons();
    }
    public void SetSlotIcons()
    {
        helmet.sprite = LobbyEquipmentManager.inst.currentHelmet.equipmentIcon;
        mainHand.sprite=LobbyEquipmentManager.inst.currentMainHand.equipmentIcon;
        offHand.sprite=LobbyEquipmentManager.inst.currentOffHand.equipmentIcon;

        var petManager = LobbyPetManager.inst;
        if (petManager.currentPet != null) { pet.sprite = petManager.currentPet.petIcon; }
        else { pet.sprite = emptySprite;}
    }
}
