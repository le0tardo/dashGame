using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool inst;

    [SerializeField] GameObject[] Zombies;
    [SerializeField] GameObject[] Bats;

    private void Awake()
    {
        inst = this;
    }


    public void SpawnZombie(Vector3 position, Quaternion rotation)
    {
        if (Zombies.Length <= 0) return;

        for (int i = 0; i < Zombies.Length; i++)
        {
            if (!Zombies[i].activeInHierarchy)
            {
                Zombies[i].transform.position = position;
                Zombies[i].transform.rotation = rotation;
                Zombies[i].SetActive(true);
                return;
            }
        }
    }
}
