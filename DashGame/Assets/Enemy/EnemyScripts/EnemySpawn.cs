using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    enum EnemyType
    {
        Zombie,
        Bat
    }

    [SerializeField] EnemyType enemyType;

    bool failSafe=false;
    private void OnEnable()
    {
        if (EnemyPool.inst == null)
        {
            return;
        }
        else
        {
            Spawn();
            failSafe = true;
        }

    }

    private void Start()
    {
       if(!failSafe) Spawn();
    }

    private void Spawn()
    {
        switch (enemyType)
        {
            case EnemyType.Zombie:
                EnemyPool.inst.SpawnZombie(transform.position,transform.rotation);
                break;
                case EnemyType.Bat:
                break;
        }

        this.gameObject.SetActive(false);
    }
}
