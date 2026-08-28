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

    [Header("Kill drops")]
    [SerializeField] GameObject drop;

    EnemyMove move;
    PlayerStats player;

    private void Start()
    {
        maxHealth = health;
        move = GetComponent<EnemyMove>();
        player = FindFirstObjectByType<PlayerStats>();

        InvokeRepeating("DealDamage",0,attackSpeed);
    }

    private void Update()
    {
        if (player != null)
        {
            float dist = Vector3.Distance(transform.position, player.transform.position);
            if (dist <= attackRange)
            {
                inCombat = true;
            }
            else
            {
                inCombat= false;
            }
        }
    }

    public void TakeDamage(float dmg)
    {
        health-=dmg;
        Mathf.Clamp(health, 0, maxHealth);

        if(dmg>0)DamagePopUpManager.inst.PopUp(transform.position, dmg.ToString());

        if (health <= 0)
        {
            GameObject pickupManager = GameObject.Find("PickupManager");

            if (drop != null && pickupManager != null)
            {
                drop.SetActive(true);
                drop.transform.SetParent(pickupManager.transform);
            }

            isDead = true;
            EnemyShatterManager.inst.ShatterZombie(transform.position);
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
