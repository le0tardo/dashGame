using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
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
    PlayerStats player;

    private void Start()
    {
        maxHealth = health;
        move = GetComponent<EnemyMove>();
        player = FindFirstObjectByType<PlayerStats>();

        InvokeRepeating("DealDamage",0,attackSpeed);
    }

    public void TakeDamage(float dmg)
    {
        health-=dmg;
        Mathf.Clamp(health, 0, maxHealth);

        if(dmg>0)DamagePopUpManager.inst.PopUp(transform.position, dmg.ToString());

        if (health <= 0)
        {
            OrbPool.inst.SpawnOrbs(xp,transform.position);

            isDead = true;
            ShatterManager.inst.ShatterZombie(transform.position);
            Die();

        }
    }

    void DealDamage()
    {
        if (!isDead && !move.isFalling)
        {
            if (inCombat)
            {
                Vector3 halfwayPosition = (transform.position + player.transform.position) * 0.5f;
                player.TakeDamage(damage, halfwayPosition);
            }
        }
    }

    void Die()
    {
        Destroy(this.gameObject);
        //this.gameObject.SetActive(false);
        //TODO recycle in enemy pool
    }
}
