using UnityEngine;

public class ZombieHeads : MonoBehaviour
{
    [SerializeField] GameObject[] heads;

    private void Start()
    {
        foreach (GameObject head in heads)
        {
            head.SetActive(false);
        }

        int r=Random.Range(0, heads.Length);
        heads[r].SetActive(true);
    }
}
