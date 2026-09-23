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
        int spawnedCount = 0;

        for (int i = 0; i < xpOrbs.Length; i++)
        {
            if (spawnedCount >= amount) break;

            if (!xpOrbs[i].activeInHierarchy)
            {
                xpOrbs[i].transform.position = pos;
                xpOrbs[i].SetActive(true);

                spawnedCount++;
            }
        }
    }

    public void SpawnStaminaOrbs(int amount, Vector3 pos)
    {
        int spawnedCount = 0;

        for (int i = 0; i < staminaOrbs.Length; i++)
        {
            if (spawnedCount >= amount) break;

            if (!staminaOrbs[i].activeInHierarchy)
            {
                staminaOrbs[i].transform.position = pos;
                staminaOrbs[i].SetActive(true);

                spawnedCount++;
            }
        }
    }

    public void SpawnHealthOrbs(int amount, Vector3 pos)
    {
        int spawnedCount = 0;

        for (int i = 0; i < healthOrbs.Length; i++)
        {
            if (spawnedCount >= amount) break;

            if (!healthOrbs[i].activeInHierarchy)
            {
                healthOrbs[i].transform.position = pos;
                healthOrbs[i].SetActive(true);

                spawnedCount++;
            }
        }
    }

}
