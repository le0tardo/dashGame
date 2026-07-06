using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [Header("Combat Stats")]
    [SerializeField] public float health;
    float maxHealth;
    [SerializeField] float damage;
    [SerializeField] float attackRange;
    [SerializeField] float attackSpeed;
    [SerializeField]bool isDead=false;

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
        if (health <= 0)
        {
            isDead = true;
            Destroy(this.gameObject);
        }
    }

    void DealDamage()
    {
        // Cleaned up logic: Only attack if the enemy is alive AND the player isn't moving/falling
        // (Grouping them with parentheses prevents C# from misinterpreting the OR '||' condition)
        if (!isDead && !move.isFalling)
        {
            if (player != null)
            {
                float dist = Vector3.Distance(transform.position, player.transform.position);
                if (dist <= attackRange)
                {
                    Vector3 halfwayPosition = (transform.position + player.transform.position) * 0.5f;
                    player.TakeDamage(damage, halfwayPosition);
                }
            }
        }
    }

}
