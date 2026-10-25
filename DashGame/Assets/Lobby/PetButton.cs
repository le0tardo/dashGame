using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PetButton : MonoBehaviour
{
    [SerializeField] PetObject pet;
    [SerializeField] Image petIcon;
    [SerializeField] RectTransform equippedMarker;
    [SerializeField] GameObject petPrefab;
    [SerializeField] AudioClip equipPetSound;

    [Header("unlock")]
    [SerializeField] bool unlocked = false;
    [SerializeField] GameObject lockedGfx;
    [SerializeField] int goldCost;
    TextMeshProUGUI costText;

    [Header("Too Poor")]
    float flashDuration = 0.33f;
    Coroutine flashRoutine = null;

    private void Start()
    {
        petIcon.sprite = pet.petIcon;

        costText = lockedGfx.GetComponentInChildren<TextMeshProUGUI>();
        costText.text = goldCost.ToString();
    }

    public void UnlockOrEquip()
    {
        if (!unlocked)
        {
            if (LobbyGold.inst.gold >= goldCost)
            {
                Unlock();
            }
            else
            {
                TooPoor();
            }
        }
        else
        {
            SetPet();
        }
    }
    public void Unlock()
    {
        if (!unlocked) { unlocked = true; }
        lockedGfx.SetActive(false);

        LobbyGold.inst.gold -= goldCost;
        LobbyGold.inst.SpendGoldVisuals(LobbyGold.inst.gold + goldCost, LobbyGold.inst.gold);
        LobbyGold.inst.UnlockFX(transform.position);

    }

    public void SetPet()
    {
        if (petPrefab != null) //outsource to petManager script TODO
        {
            petPrefab.SetActive(true);
            LobbyPetManager.inst.SetPet(pet);
            AudioManager.inst.PlayCustomSound(pet.petSound, 0.5f);
        }

        AudioManager.inst.PlayCustomSound(equipPetSound, 0.5f);
        equippedMarker.transform.position = transform.position;
    }

    public void TooPoor()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }
        flashRoutine = StartCoroutine(FlashRoutine(Color.white, Color.red));

        LobbyGold.inst.TooPoor();
    }

    IEnumerator FlashRoutine(Color defaultColor, Color flashColor)
    {
        float elapsed = 0;

        while (elapsed < flashDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float progress = Mathf.Clamp01(elapsed / flashDuration);

            float pingPongProgress = Mathf.PingPong(progress * 4f, 1f);

            costText.color = Color.Lerp(defaultColor, flashColor, pingPongProgress);

            yield return null;
        }

        costText.color = defaultColor;
        flashRoutine = null;
    }
}
