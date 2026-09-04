using UnityEngine;

public class ShatterManager : MonoBehaviour
{
    [Header("Enemies")]
    [SerializeField] EnemyShatter[] batShatter;
    [SerializeField] EnemyShatter[] zombieShatter;
    [SerializeField] EnemyShatter[] murret1Shatter;

    [Header("Props")]
    [SerializeField] EnemyShatter[] potShatter;
    [SerializeField] EnemyShatter[] crateShatter;
    [SerializeField] EnemyShatter[] barrelShatter;

    public static ShatterManager inst;
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

    public void ShatterCrate(Vector3 pos)
    {
        for (int i = 0; i < crateShatter.Length; i++)
        {
            if (!crateShatter[i].isActiveAndEnabled)
            {
                crateShatter[i].gameObject.SetActive(true);
                crateShatter[i].Shatter(pos);
                return;
            }
        }
    }
    public void ShatterBarrel(Vector3 pos)
    {
        for (int i = 0; i < barrelShatter.Length; i++)
        {
            if (!barrelShatter[i].isActiveAndEnabled)
            {
                barrelShatter[i].gameObject.SetActive(true);
                barrelShatter[i].Shatter(pos);
                return;
            }
        }
    }


    public void ShatterObject(Vector3 pos, string obj)
    {

        switch (obj)
        {
            case "pot":
                print("shatter pot!");
                break;
            case "crate":
                print("shatter crate!");
                break;
            case "barrel":
                print("shatter barrel!");
                break;

            default:
                print(obj+" unknown reference in shatter manager.");
                break;
        }
    }
}
