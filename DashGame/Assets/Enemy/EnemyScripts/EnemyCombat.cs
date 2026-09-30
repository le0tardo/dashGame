using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    enum EnemyType
    {
        Zombie,
        Skeleton,
        Slime
    }

    [SerializeField] string enemyName;
    [SerializeField] EnemyType enemyType;

    [Header("Combat Stats")]
    [SerializeField] public float health;
    float maxHealth;
    [SerializeField] float damage;
    [SerializeField] float attackRange;
    [SerializeField] float attackSpeed;
    public bool inCombat = false;
    [SerializeField]bool isDead=false;
    [SerializeField] int xp;

    EnemyMove move;
    EnemyAnimations animations;
    PlayerStats player;

    private void Start()
    {
        maxHealth = health;
        move = GetComponent<EnemyMove>();
        animations = GetComponentInChildren<EnemyAnimations>();
        player = FindFirstObjectByType<PlayerStats>();

        InvokeRepeating("DealDamage",0,attackSpeed);
    }

    public void TakeMeleeDamage(float dmg)
    {
        animations.Flash();
        animations.MeleeHitAnimation();
        TakeDamage(dmg);
        HitFxManager.inst.HitFX1(transform.position, 0.5f);
    }

    public void TakeDamage(float dmg)
    {
        dmg = Mathf.Round(dmg);
        health-=dmg;
        Mathf.Clamp(health, 0, maxHealth);

        if(dmg>0)DamagePopUpManager.inst.PopUp(transform.position, dmg.ToString());

        if (health <= 0)
        {
            OrbPool.inst.SpawnOrbs(xp,transform.position);

            isDead = true;
            switch (enemyType)
            {
                case EnemyType.Zombie:
                    ShatterManager.inst.ShatterZombie(transform.position);
                    break;
                case EnemyType.Skeleton:
                    ShatterManager.inst.ShatterSkeleton(transform.position);
                    break;
                case EnemyType.Slime:
                    ShatterManager.inst.ShatterSlime(transform.position);
                    break;
                default :
                    print("missing enum state");
                    break;
            }
            Die();
        }
    }

    void DealDamage()
    {
        float dist = Vector3.Distance(player.transform.position,transform.position);
        if (dist <= attackRange && LevelManager.inst.health>0) inCombat = true; else inCombat = false;

        if (!isDead && !move.isFalling)
        {
            if (inCombat)
            {
                Vector3 halfwayPosition = (transform.position + player.transform.position) * 0.5f;
                player.TakeDamage(damage, halfwayPosition);
            }
        }
    }

    public void Die()
    {
        //Destroy(this.gameObject);

        // Tell the room this enemy is dead so it won't be re-enabled
        if (RoomManager.inst.currentRoom != null)
        {
            if (RoomManager.inst.currentRoom.TryGetComponent(out RoomBehaviour room))
            {
               if(room.enemies.Contains(gameObject)) room.enemies.Remove(gameObject);
            }
        }
        this.gameObject.SetActive(false);
    }
}
