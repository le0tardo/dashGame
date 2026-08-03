using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;

public class TurretEnemy : MonoBehaviour, IHittable
{
    public Transform playerTransform;
    [SerializeField] float turnSpeed = 90f;
    [SerializeField] float attackSpeed = 1f;
    [SerializeField] bool bullsEye=false;
    [SerializeField] GameObject[] bullet;
    PlayerMove target;
    Vector3 targetDirection;

    float bounce = 0.85f;
    public float hitBounce =>bounce;

    [Header("CombatStats")]
    public float health = 10f;
    float maxHealth;
    private void Start()
    {
        target = Object.FindFirstObjectByType<PlayerMove>();
        if (target != null)
        {
            playerTransform = target.transform;
        }

        maxHealth = health;

        InvokeRepeating("FireBullet", 0, attackSpeed);
    }

    public void OnHit(Vector3 hitPos, float pwr)
    {
        Vector3 hitDirection = (hitPos - transform.position);
        hitDirection.y = 0f;
        hitDirection.Normalize();

        Vector3 enemyForward = transform.forward;
        enemyForward.y = 0f;
        enemyForward.Normalize();

        float dot = Vector3.Dot(hitDirection, enemyForward);
        dot=Mathf.Abs(dot);

        float dotMod = 1f;
        if (dot <= 0.8f)
        {
            dotMod = 2f;
        }

        float finalDamage = Mathf.Floor(pwr / 10) * dotMod;
        GetHit(finalDamage);
        
        Vector3 fxPos = (transform.position + hitPos) / 2;
        HitFxManager.inst.HitFX1(fxPos, pwr / 10);
        CameraShake.inst.Shake(0.15f, 1.5f);
        AudioManager.inst.PlayMetalImpactSound(pwr/2);
    }
    private void Update()
    {
        if (playerTransform == null) return;

        targetDirection = playerTransform.position - transform.position;
        targetDirection.y = 0;

        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
        float angleDifference = Vector3.Angle(transform.forward, targetDirection);
        if (angleDifference < 11.25f)
        {
            bullsEye = true;
        }
        else
        {
            bullsEye = false;
        }
    }

    void FireBullet()
    {
        if (!bullsEye) return;

        for (int i = 0; i < bullet.Length; i++)
        {
            if (!bullet[i].activeInHierarchy)
            {
                bullet[i].SetActive(true);
                TurretBullet tb = bullet[i].GetComponent<TurretBullet>();
                tb.SetDirection(targetDirection);
                return;
            }
        }
    }

    public void GetHit(float dmg)
    {
        health -= dmg;

        if (dmg > 0) DamagePopUpManager.inst.PopUp(transform.position, dmg.ToString());

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(this.gameObject);
    }
}
