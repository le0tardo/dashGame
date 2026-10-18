using UnityEngine;
using UnityEngine.UI;

public class ChosenUpgradeFX : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Image icon;
    [SerializeField] AudioClip upgSound;
    Vector3 startPos;
    private void Awake()
    {
        anim=GetComponent<Animator>();
        icon = GetComponent<Image>();
        startPos = transform.position;
    }
    public void ChoosenUpgradeFX(Vector3 uiPosition, UpgradeObject upg)
    {
        AudioManager.inst.PlayCustomSound(upgSound,0.5f);
        transform.position = uiPosition;
        icon.sprite = upg.upgradeIcon;
        anim.Play("fade", -1, 0f);
        Invoke(nameof(ResetPosition), 1f);
    }

    private void ResetPosition()
    {
        transform.position = startPos;
    }
}
