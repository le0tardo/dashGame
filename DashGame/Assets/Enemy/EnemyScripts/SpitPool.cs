using Unity.Mathematics;
using UnityEngine;

public class SpitPool : MonoBehaviour
{
    public static SpitPool inst;

    [SerializeField] GameObject[] spit;
    [SerializeField] ParticleSystem[] splash;

    private void Awake()
    {
        inst = this;
    }

    public void Spit(Vector3 pos, Quaternion rot)
    {
        for (int i = 0; i < spit.Length; i++)
        {
            if (!spit[i].activeInHierarchy)
            {
                spit[i].transform.position = pos;
                spit[i].transform.rotation = rot;
                spit[i].SetActive(true);
                return;
            }
        }
    }

    public void SpitHit(Vector3 pos, quaternion rot)
    {
        for(int i = 0;i < splash.Length;i++)
        {
            if (!splash[i].gameObject.activeInHierarchy)
            {
                splash[i].transform.position = pos;
                splash[i].transform.rotation = rot * Quaternion.Euler(-45f, 0f, 0f);
                splash[i].gameObject.SetActive(true);
                splash[i].Play();
                return;
            }
        }
    }
}
