using UnityEngine;

public class LobbyEquipmentManager : MonoBehaviour
{
    public static LobbyEquipmentManager inst;

    [SerializeField] Transform helmetSlot;
    [SerializeField] Transform mainHandSlot;
    [SerializeField] Transform offHandSlot;

    [SerializeField] public HeroEquipmentObject currentHelmet;
    [SerializeField] public HeroEquipmentObject currentMainHand;
    [SerializeField] public HeroEquipmentObject currentOffHand;

    //a new scriptable object class here that tracks current bonuses and carries over to main scene
    //or use existing??

    private void Awake()
    {
        inst = this;
    }

    private void Start()
    {
        Equip();
    }
    void Equip()
    {
        if (currentHelmet != null)
        {
            GameObject newHelmet = Instantiate(currentHelmet.equipmentPrefab,helmetSlot);
        }
        if (currentMainHand != null)
        {
            GameObject newMainHand = Instantiate(currentMainHand.equipmentPrefab, mainHandSlot);
        }
        if (currentOffHand != null)
        {
            GameObject newOffHand = Instantiate(currentOffHand.equipmentPrefab, offHandSlot);
        }
    }

    public void EquipHelmet(HeroEquipmentObject helmet)
    {
        currentHelmet = helmet;
        
    }

}
