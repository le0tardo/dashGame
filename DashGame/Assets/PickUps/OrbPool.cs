using UnityEngine;

public class OrbPool : MonoBehaviour
{
    public static OrbPool inst;
    [SerializeField] GameObject[] xpOrbs;
    [SerializeField] GameObject[] staminaOrbs;
    [SerializeField] GameObject[] healthOrbs;

    private void Awake()
    {
        inst = this;
    }

    public void SpawnOrbs(int amount, Vector3 pos)
    {
        if(amount>xpOrbs.Length)amount= xpOrbs.Length;

        for (int i = 0; i < amount; i++) 
        {
            if (!xpOrbs[i].activeInHierarchy)
            {
                xpOrbs[i].transform.position = pos;
                xpOrbs[i].gameObject.SetActive(true);
            }
        }
    }

    public void SpawnStaminaOrbs(int amount, Vector3 pos)
    {
        if (amount > staminaOrbs.Length) amount = staminaOrbs.Length;

        for (int i = 0; i < amount; i++)
        {
            if (!staminaOrbs[i].activeInHierarchy)
            {
                staminaOrbs[i].transform.position = pos;
                staminaOrbs[i].gameObject.SetActive(true);
            }
        }
    }

    public void SpawnHealthOrbs(int amount, Vector3 pos)
    {
        if(amount>healthOrbs.Length)amount = healthOrbs.Length;
        for(int i = 0;i < amount; i++)
        {
            if (!healthOrbs[i].activeInHierarchy)
            {
                healthOrbs[i].transform.position = pos;
                healthOrbs[i].SetActive(true);
            }
        }
    }

}
