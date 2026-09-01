using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemyShatterManager : MonoBehaviour
{
    [Header("Enemies")]
    [SerializeField] EnemyShatter[] batShatter;
    [SerializeField] EnemyShatter[] zombieShatter;
    [SerializeField] EnemyShatter[] murret1Shatter;

    [Header("Props")]
    [SerializeField] EnemyShatter[] potShatter;

    public static EnemyShatterManager inst;
    private void Awake()
    {
        inst = this;
    }
    public void ShatterBat(Vector3 pos)
    {
        for (int i = 0; i < batShatter.Length; i++)
        {
            if (!batShatter[i].isActiveAndEnabled)
            {
                batShatter[i].gameObject.SetActive(true);
                batShatter[i].Shatter(pos);
                return;
            }
        }
    }

    public void ShatterZombie(Vector3 pos)
    {
        for (int i = 0; i < zombieShatter.Length; i++)
        {
            if (!zombieShatter[i].isActiveAndEnabled)
            {
                zombieShatter[i].gameObject.SetActive(true);
                zombieShatter[i].Shatter(pos);
                return;
            }
        }
    }

    public void ShatterMurret1(Vector3 pos)
    {
        for (int i = 0; i < murret1Shatter.Length; i++)
        {
            if (!murret1Shatter[i].isActiveAndEnabled)
            {
                murret1Shatter[i].gameObject.SetActive(true);
                murret1Shatter[i].Shatter(pos);
                return;
            }
        }
    }

    public void ShatterPot(Vector3 pos)
    {
        for(int i = 0; i < potShatter.Length; i++)
        {
            if (!potShatter[i].isActiveAndEnabled)
            {
                potShatter[i].gameObject.SetActive(true);
                potShatter[i].Shatter(pos);
                return;
            }
        }
    }
}
