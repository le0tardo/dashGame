using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamagePopUp : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI text;
    Animator anim;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private float moveSpeed = 150f;

    [SerializeField] Color red;
    [SerializeField] Color green;

    private void Awake()
    {
        if(rectTransform==null)rectTransform = GetComponent<RectTransform>();
        if(anim==null)anim = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        Invoke("Sleep", 0.5f);
    }
    private void Update()
    {
        rectTransform.anchoredPosition += Vector2.up * (moveSpeed * Time.deltaTime);
    }
    void Sleep()
    {
        this.gameObject.SetActive(false);
    }

    public void ShowDamage(string dmg, bool crit)
    {
        text.color = Color.white;
        text.text = dmg;
        if (crit) //TODO
        {
            anim.SetTrigger("crit");
        }
    }

    public void ShowDamageRed(string dmg, bool crit)
    {
        text.color = red;
        text.text = dmg;
        if (crit) //TODO
        {
            anim.SetTrigger("crit");
        }
    }

    public void ShowHeal(string dmg)
    {
        text.color = green;
        text.text = dmg;
    }

}
