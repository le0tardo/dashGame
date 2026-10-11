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

    [SerializeField] GameObject EquipedHelmetPrefab;
    [SerializeField] GameObject EquipedMainHandPrefab;
    [SerializeField] GameObject EquipedOffHandPrefab;

    //a new scriptable object class here that tracks current bonuses and carries over to main scene
    //or use existing??

    private void Awake()
    {
        inst = this;
    }

    private void Start()
    {
        EquipAll();
    }
    void EquipAll()
    {
        if (currentHelmet != null)
        {
            EquipedHelmetPrefab = Instantiate(currentHelmet.equipmentPrefab,helmetSlot); //make this have all the transforms of the parent object?
        }
        if (currentMainHand != null)
        {
            EquipedMainHandPrefab = Instantiate(currentMainHand.equipmentPrefab, mainHandSlot);
        }
        if (currentOffHand != null)
        {
            EquipedOffHandPrefab = Instantiate(currentOffHand.equipmentPrefab, offHandSlot);
        }
    }

    public void EquipHelmet(HeroEquipmentObject helmet)
    {
        currentHelmet = helmet;
        
    }

    public void Equip(HeroEquipmentObject equipment)
    {

        switch (equipment.equipmentSlot)
        {
            case HeroEquipmentObject.EquipmentSlot.Helmet:

                Destroy(EquipedHelmetPrefab);
                EquipedHelmetPrefab = null;
                currentHelmet = equipment;
                EquipedHelmetPrefab = Instantiate(currentHelmet.equipmentPrefab, helmetSlot);

            break;

            case HeroEquipmentObject.EquipmentSlot.MainHand:
                Destroy(EquipedMainHandPrefab);
                EquipedMainHandPrefab= null;
                currentMainHand = equipment;
                EquipedMainHandPrefab = Instantiate(currentMainHand.equipmentPrefab, mainHandSlot);

            break;

            case HeroEquipmentObject.EquipmentSlot.OffHand:
                Destroy(EquipedOffHandPrefab);
                EquipedOffHandPrefab= null;
                currentOffHand = equipment;
                EquipedOffHandPrefab = Instantiate(currentOffHand.equipmentPrefab, offHandSlot);
            break;

            default:
                print("Error assigning ENUM state on equipment");
            break;
        }
    }

}
