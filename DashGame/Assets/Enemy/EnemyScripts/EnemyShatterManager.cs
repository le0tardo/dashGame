using UnityEngine;

public class EnemyShatterManager : MonoBehaviour
{
    [SerializeField] EnemyShatter[] batShatter;
    [SerializeField] EnemyShatter[] zombieShatter;

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
}
