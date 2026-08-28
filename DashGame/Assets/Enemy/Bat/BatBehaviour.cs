using UnityEngine;

public class BatBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerStats player;
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private Vector3 playerPosition;
    [SerializeField] float offsetRadius = 1f;
    Vector2 randomOffset;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] GameObject drop;
    [SerializeField] BatAnimations batAnim;

    [Header("State")]
    [SerializeField] private bool seesPlayer = false;
    [SerializeField] private bool inRange = false;
    [SerializeField] private bool knockedBack=false;
    [SerializeField] private bool canTakeDamage = true;

    [Header("Stats")]
    [SerializeField] private float health = 10f;
    [SerializeField] private float damage = 5f;
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private float attackRange = 1.5f;

    [Header("Movement & Flight Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float visionRange = 15f;
    [SerializeField] private float waveFrequency = 5f;  // Speed of the squiggly oscillation
    [SerializeField] private float waveAmplitude = 1.5f; // Width/height of the sway

    private float waveTimer = 0.1f;


    private void Awake()
    {
        if (player == null) player = FindAnyObjectByType<PlayerStats>();
        playerMove=player.gameObject.GetComponent<PlayerMove>();

        batAnim = GetComponentInChildren<BatAnimations>();
        if (batAnim == null) { print("no anim scritp for bat!"); }

        if (attackSpeed < 0.1f) attackSpeed = 1f;
        float r=Random.value;
        InvokeRepeating(nameof(TryAttack), 1f+r, attackSpeed);
        randomOffset = Random.insideUnitCircle * offsetRadius;
    }

    private void Update()
    {
        if (player == null) return;

        playerPosition = player.transform.position + new Vector3(randomOffset.x, 0f, randomOffset.y);

        CheckLineOfSightAndRange();

        float dist = Vector3.Distance(playerPosition, transform.position);

        if (seesPlayer && !inRange && dist>3f)
        {
            MoveInSquigglyLine();
        }
        else
        {
            if(batAnim.moving)batAnim.moving = false;
        }
    }

    private void CheckLineOfSightAndRange()
    {
        Vector3 dirToPlayer = playerPosition - transform.position;
        float distanceToPlayer = dirToPlayer.magnitude;

        // 1. Range check
        inRange = distanceToPlayer <= (attackRange*2); //idk sine wave offset maybe

        // 2. Vision check (Raycast towards player position)
        if (distanceToPlayer <= visionRange)
        {
            // Raycast only checks against obstacle layers (walls, doors, terrain)
            if (!Physics.Raycast(transform.position, dirToPlayer.normalized, distanceToPlayer, obstacleMask))
            {
                seesPlayer = true;
                return;
            }
        }

        seesPlayer = false;
    }

    private void MoveInSquigglyLine()
    {
        //check anim bool here
        if(!batAnim.moving)batAnim.moving = true;

        waveTimer += Time.deltaTime * waveFrequency;

        // 1. Calculate direction to player and lock Y to 0
        Vector3 forwardDir = playerPosition - transform.position;
        forwardDir.y = 0f;
        forwardDir = forwardDir.normalized;

        // 2. Get perpendicular side vector strictly in the X-Z plane
        Vector3 rightDir = Vector3.Cross(Vector3.up, forwardDir).normalized;

        // 3. Side-to-side sine wave offset (horizontal only)
        Vector3 waveOffset = rightDir * Mathf.Sin(waveTimer) * waveAmplitude;

        // 4. Combine forward motion with sideways sway
        Vector3 targetVelocity = (forwardDir * moveSpeed) + waveOffset;

        // Move horizontally
        transform.position += targetVelocity * Time.deltaTime;

        // Face forward direction without pitching up/down
        if (forwardDir != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(forwardDir), Time.deltaTime * 10f);
        }
    }

    private void TryAttack()
    {
        if (seesPlayer && inRange && player != null)
        {
            Vector3 knockbackpos = (transform.position - playerPosition) / 2;
            player.TakeLightDamage(damage, knockbackpos);
            batAnim.BatAttack();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerMove.currentVelocity.magnitude > 10)
            {
                if (canTakeDamage)
                {
                    TakeDamage(playerMove.currentVelocity.magnitude);
                    canTakeDamage = false;
                }

            }
        }
    }

    void TakeDamage(float dmg)
    {
        dmg = Mathf.Floor(dmg/10);
        health -= dmg;

        AudioManager.inst.PlayEnemyImpactSound(0.1f+dmg/10);

        if (dmg > 0) DamagePopUpManager.inst.PopUp(transform.position, dmg.ToString());

        Vector3 hitPos=(transform.position + playerPosition)/2;
        HitFxManager.inst.HitFX1(hitPos,dmg/2);

        if (health <= 0)
        {
            Die();
        }
        else
        {
            float knockbackDistance = 2.5f;

            if (!knockedBack)
            {
                Vector3 pushDir = transform.position - player.transform.position;
                pushDir.y = 0f;
                Vector3 targetPosition = transform.position + (pushDir.normalized * knockbackDistance);
                StopCoroutine(nameof(KnockbackRoutine));
                StartCoroutine(KnockbackRoutine(targetPosition));

                batAnim.BatHit();
            }
        }
    }

    private System.Collections.IEnumerator KnockbackRoutine(Vector3 targetPosition)
    {
        knockedBack = true;

        Vector3 startPosition = transform.position;
        float knockbackDuration = 0.1f;
        float elapsed = 0f;

        // Fast linear interpolation towards targetPosition
        while (elapsed < knockbackDuration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / knockbackDuration;

            // SmoothStep makes the start snappy and decelerates slightly at the end
            float t = Mathf.SmoothStep(0f, 1f, percent);

            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        transform.position = targetPosition; // Ensure exact final position
        knockedBack = false;
        canTakeDamage = true;
    }

    void Die()
    {
        GameObject pickupManager = GameObject.Find("PickupManager");

        if (drop != null && pickupManager != null)
        {
            drop.SetActive(true);
            drop.transform.SetParent(pickupManager.transform);
        }

        EnemyShatterManager.inst.ShatterBat(transform.position);
        Destroy(this.gameObject);
    }

    // Visualizes vision & attack range in Unity Editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}