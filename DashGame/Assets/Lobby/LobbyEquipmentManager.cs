using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class LobbyEquipmentManager : MonoBehaviour
{
    public static LobbyEquipmentManager inst;

    [SerializeField] Transform helmetSlot;
    [SerializeField] Transform mainHandSlot;
    [SerializeField] Transform offHandSlot;
    [SerializeField] Transform buckleSlot;

    [SerializeField] public HeroEquipmentObject currentHelmet;
    [SerializeField] public HeroEquipmentObject currentMainHand;
    [SerializeField] public HeroEquipmentObject currentOffHand;
    [SerializeField] public HeroEquipmentObject currentBuckle;

    [SerializeField] GameObject EquipedHelmetPrefab;
    [SerializeField] GameObject EquipedMainHandPrefab;
    [SerializeField] GameObject EquipedOffHandPrefab;
    [SerializeField] GameObject EquipedBucklePrefab;

    [SerializeField] EquipmentSlots slots;

    //a new scriptable object class here that tracks current bonuses and carries over to main scene
    //or use existing??
    [SerializeField] AudioClip equipSound;
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
            EquipedHelmetPrefab = Instantiate(currentHelmet.equipmentPrefab,helmetSlot);
        }
        if (currentMainHand != null)
        {
            EquipedMainHandPrefab = Instantiate(currentMainHand.equipmentPrefab, mainHandSlot);
        }
        if (currentOffHand != null)
        {
            EquipedOffHandPrefab = Instantiate(currentOffHand.equipmentPrefab, offHandSlot);
        }
        if (currentBuckle != null)
        {
            EquipedBucklePrefab = Instantiate(currentBuckle.equipmentPrefab, buckleSlot);
        }
    }


    public void Equip(HeroEquipmentObject equipment)
    {

        switch (equipment.equipmentSlot)
        {
            case HeroEquipmentObject.EquipmentSlot.Helmet:

                LobbyStatManager.inst.UnequipItem(currentHelmet);

                Destroy(EquipedHelmetPrefab);
                EquipedHelmetPrefab = null;
                currentHelmet = equipment;
                EquipedHelmetPrefab = Instantiate(currentHelmet.equipmentPrefab, helmetSlot);

                //add actual stats here?

            break;

            case HeroEquipmentObject.EquipmentSlot.MainHand:

                LobbyStatManager.inst.UnequipItem(currentMainHand);

                Destroy(EquipedMainHandPrefab);
                EquipedMainHandPrefab= null;
                currentMainHand = equipment;
                EquipedMainHandPrefab = Instantiate(currentMainHand.equipmentPrefab, mainHandSlot);

            break;

            case HeroEquipmentObject.EquipmentSlot.OffHand:

                LobbyStatManager.inst.UnequipItem(currentOffHand);

                Destroy(EquipedOffHandPrefab);
                EquipedOffHandPrefab= null;
                currentOffHand = equipment;
                EquipedOffHandPrefab = Instantiate(currentOffHand.equipmentPrefab, offHandSlot);

            break;

            case HeroEquipmentObject.EquipmentSlot.Buckle:

                LobbyStatManager.inst.UnequipItem(currentBuckle);

                Destroy(EquipedBucklePrefab);
                EquipedBucklePrefab= null;
                currentBuckle = equipment;
                EquipedBucklePrefab = Instantiate(currentBuckle.equipmentPrefab, buckleSlot);

            break;

            default:
                print("Error assigning ENUM state on equipment");
            break;
        }

        AudioManager.inst.PlayCustomSound(equipSound,1f);
        LobbyStatManager.inst.EquipItem(equipment);
        slots.SetSlotIcons();
    }


}
