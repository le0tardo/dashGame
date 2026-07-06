using System.Diagnostics.Contracts;
using UnityEngine;

public class TurretEnemy : MonoBehaviour
{
    public Transform playerTransform;
    [SerializeField] float turnSpeed = 90f;
    [SerializeField] float attackSpeed = 1f;
    [SerializeField] bool bullsEye=false;
    [SerializeField] GameObject[] bullet;
    PlayerMove target;
    Vector3 targetDirection;

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
