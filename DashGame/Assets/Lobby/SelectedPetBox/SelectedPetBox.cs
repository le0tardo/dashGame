using TMPro;
using UnityEngine;

public class SelectedPetBox : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI level;
    [SerializeField] TextMeshProUGUI attackPower;
    [SerializeField] TextMeshProUGUI attackSpeed;

    LobbyEquipmentManager equipManager;
    LobbyPetManager petManager;

    Vector3 startTransform;
    Vector3 endTransform;

    private void Start()
    {
        equipManager = LobbyEquipmentManager.inst;
        petManager = LobbyPetManager.inst;

        startTransform = this.transform.position;
        endTransform = new Vector3(-99, -99, -99);

        PetSelected();
    }

    public void PetSelected()
    {
        if (petManager.currentPet != null)
        {
            ShowBox();
            title.text = petManager.currentPet.petName;
            level.text = "lvl "+petManager.currentPet.petLevel.ToString();
            attackPower.text = "Power: "+petManager.currentPet.petAttackPower.ToString();
            attackSpeed.text = "Speed: "+petManager.currentPet.petAttackSpeed.ToString();
        }
        else
        {
            HideBox();
        }

    }

    void HideBox()
    {
        transform.position = endTransform;
    }

    void ShowBox()
    {
        transform.position = startTransform;
    }
}
