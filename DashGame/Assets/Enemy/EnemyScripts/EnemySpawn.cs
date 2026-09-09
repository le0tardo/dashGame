using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    enum EnemyType
    {
        Zombie,
        Bat
    }

    [SerializeField] EnemyType enemyType;
    private void OnEnable()
    {
        if (EnemyPool.inst == null) return;

        Spawn();
    }

    private void Start()
    {
        Spawn();
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
    }
}
