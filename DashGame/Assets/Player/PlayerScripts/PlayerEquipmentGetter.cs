using UnityEngine;

public class PlayerEquipmentGetter : MonoBehaviour
{
    [SerializeField] SessionData sessionData;

    [Header("Equipment Objects")]
    [SerializeField] HeroEquipmentObject helmet;
    [SerializeField] HeroEquipmentObject mainHand;
    [SerializeField] HeroEquipmentObject offHand;

    [Header("Equipment Slots")]
    [SerializeField] Transform helmetSlot;
    [SerializeField] Transform mainHandSlot;
    [SerializeField] Transform offHandSlot;

    [Header("Equipment Prefabs")]
    [SerializeField] GameObject equippedHelmet;
    [SerializeField] GameObject equippedMainHand;
    [SerializeField] GameObject equippedOffHand;

    private void Start()
    {
        if (sessionData == null || sessionData.helmet==null || sessionData.mainHand==null||sessionData.offHand==null) return;

        EquipItems();
    }

    void EquipItems()
    {
        Destroy(equippedHelmet);
        equippedHelmet = null;
        equippedHelmet=Instantiate(sessionData.helmet.equipmentPrefab,helmetSlot);

        Destroy(equippedMainHand);
        equippedMainHand = null;
        equippedMainHand = Instantiate(sessionData.mainHand.equipmentPrefab, mainHandSlot);

        Destroy(equippedOffHand);
        equippedOffHand = null;
        equippedOffHand = Instantiate(sessionData.offHand.equipmentPrefab, offHandSlot);
    }
}
