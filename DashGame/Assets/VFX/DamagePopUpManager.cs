using UnityEngine;

public class DamagePopUpManager : MonoBehaviour
{
    public static DamagePopUpManager inst;

    [SerializeField] DamagePopUp[] popUps;

    private void Awake()
    {
        inst = this;
    }

    public void PopUp(Vector3 position,string dmg)
    {
        for (int i = 0; i < popUps.Length; i++)
        {
            if (!popUps[i].gameObject.activeInHierarchy)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(position);
                screenPos.y += 50f;
                popUps[i].transform.position = screenPos;
                popUps[i].gameObject.SetActive(true);
                popUps[i].ShowDamage(dmg.ToString(),false);
                return;
            }
        }
    }

    public void PopUpRed(Vector3 position, string dmg)
    {
        for (int i = 0; i < popUps.Length; i++)
        {
            if (!popUps[i].gameObject.activeInHierarchy)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(position);
                screenPos.y += 50f;
                popUps[i].transform.position = screenPos;
                popUps[i].gameObject.SetActive(true);
                popUps[i].ShowDamageRed(dmg.ToString(), false);
                return;
            }
        }
    }

}
