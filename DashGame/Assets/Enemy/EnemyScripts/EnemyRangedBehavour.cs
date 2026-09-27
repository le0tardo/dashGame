using UnityEngine;

public class EnemyRangedBehavour : MonoBehaviour, IHittable
{
    [Header("Refereces")]
    [SerializeField] PlayerStats player;
    [SerializeField] Transform playerTransform;
    [SerializeField] Vector3 playerPosition;
    [SerializeField] public bool aimed=false;
    [SerializeField] public bool hurt=false;
    [SerializeField] GameObject maggot;
    [SerializeField] FlashRed flash;
    [SerializeField] Transform projectileOrigin;

    [SerializeField] float bounce = 0.5f;
    public float hitBounce => bounce;

    [Header("Combat stats")]
    [SerializeField] float attackSpeed = 1f;
    [SerializeField] float attackPower = 1f;
    [SerializeField] float health = 1f;
    [SerializeField] float attackRange=15f;
    [SerializeField] int xp = 5;

    [Header("Aiming")]
    [SerializeField] private float turnSpeed = 360f;
    [SerializeField] private float aimThreshold = 12f;


    private void OnEnable()
    {
        if(player==null)player = LevelManager.inst.playerStats;
        playerTransform = player.gameObject.transform;

        aimed = false;
    }

    private void Update()
    {
        if (player != null && playerTransform!=null)
        {
            

            playerPosition = playerTransform.position;

            Vector3 targetDirection = playerPosition - maggot.transform.position;
            targetDirection.y = 0f;

            if (targetDirection == Vector3.zero) return;

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            float angleDifference = Quaternion.Angle(maggot.transform.rotation, targetRotation);

            float dist = Vector3.Distance(transform.position, playerPosition);
            if (dist <= attackRange)
            {
                RotateTowardsPlayer(targetRotation);
                aimed = angleDifference <= aimThreshold;
            }


        }
    }

    void RotateTowardsPlayer(Quaternion targetRotation)
    {
        maggot.transform.rotation = Quaternion.RotateTowards(maggot.transform.rotation,targetRotation,turnSpeed * Time.deltaTime);
    }


    public void Shoot()
    {
        print("shooting at player");
        SpitPool.inst.Spit(projectileOrigin.position, projectileOrigin.transform.rotation);
    }

    public void OnHit(Vector3 hitPosition, float power)
    {
        hurt = true;

        Vector3 fxPos = (transform.position + hitPosition) / 2;
        HitFxManager.inst.HitFX1(fxPos, power / 10);
        CameraShake.inst.Shake(0.1f, 1f);
        AudioManager.inst.PlayEnemyImpactSound(power);

        flash.Flash();

        float dealtDamage = Mathf.Floor(power/10);
        health -= dealtDamage;
        print("ranged enemy took " + dealtDamage + " damage");

        if (health <= 0)
        {
            Die();
        }

    }

    void Die()
    {
        ShatterManager.inst.ShatterSpitter(transform.position);

        Vector3 spawnOrbPos = new Vector3(transform.position.x, -0.5f, transform.position.z);
        OrbPool.inst.SpawnOrbs(xp,spawnOrbPos);
        this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
