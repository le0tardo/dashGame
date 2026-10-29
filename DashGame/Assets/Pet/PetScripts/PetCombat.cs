using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetCombat : MonoBehaviour
{
    [SerializeField] private PetObject pet;
    [SerializeField] public Transform target = null;
    [SerializeField] private bool inCombat = false;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float baseAttackSpeed = 3f;

    private RoomBehaviour currentRoom;
    private Coroutine attackRoutine;

    //for enemy in line of sight bool
    [SerializeField] LayerMask petBlockLayer;

    private void Start()
    {
        if (pet != null)
        {
            attackDamage = pet.petAttackPower;
            attackSpeed = pet.petAttackSpeed / 10f;
        }
    }

    private void HandleRoomChanged()
    {
        inCombat = false;
        target = null;
        StopAttackRoutine();

        if (RoomManager.inst != null && RoomManager.inst.currentRoom != null)
        {
            currentRoom = RoomManager.inst.currentRoom.GetComponent<RoomBehaviour>();
        }
    }

    private void Update()
    {
        if (currentRoom != null && currentRoom.enemies.Count > 0)
        {
            // clean up destroyed target
            if (target == null || !target.gameObject.activeInHierarchy)
            {
                target = GetFirstValidEnemy();
                if (target == null) return; // no active enemies left
            }

            if (!inCombat)
            {
                inCombat = true;
                StartAttackRoutine();
            }
        }
        else if (inCombat)
        {
            // reset state when room is cleared
            inCombat = false;
            target = null;
            StopAttackRoutine();
        }
    }

    private Transform GetFirstValidEnemy()
    {
        for (int i = 0; i < currentRoom.enemies.Count; i++)
        {
            var enemy = currentRoom.enemies[i];

            if (enemy != null && enemy.gameObject.activeInHierarchy)
            {
                // Only select enemy if NOT blocked by a wall on petBlockLayer
                if (!IsBlockedByWall(enemy.transform))
                {
                    return enemy.transform;
                }
            }
        }
        return null; // No valid, unblocked enemies found in the room
    }

    private bool IsBlockedByWall(Transform enemyTarget)
    {
        if (enemyTarget == null) return false;

        // Elevate start and end slightly off the floor (chest height) to avoid ground clipping
        Vector3 start = transform.position + Vector3.up * 0.5f;
        Vector3 end = enemyTarget.position + Vector3.up * 0.5f;

        // Linecast returns true ONLY if a collider on petBlockLayer sits between start and end
        return Physics.Linecast(start, end, petBlockLayer);
    }
    public void DealDamage()
    {
        if (target != null)
        {
            IEnemy enemy = target.gameObject.GetComponent<IEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
            }
        }
    }
    private void StartAttackRoutine()
    {
        StopAttackRoutine();
        attackRoutine = StartCoroutine(AttackLoop());
    }

    private void StopAttackRoutine()
    {
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            attackRoutine = null;
        }
    }

    private IEnumerator AttackLoop()
    {
        while (inCombat && target != null)
        {
            // 1. Calculate overall attack delay base
            float speedMultiplier = Mathf.Max(0.1f, attackSpeed);
            float attackDelay = Mathf.Max(0.1f, baseAttackSpeed / speedMultiplier);

            // 2. Define windup vs recovery split (e.g. 30% windup, 70% cooldown)
            float windupTime = attackDelay * 0.3f;
            float recoveryTime = attackDelay * 0.7f;

            // --- WINDUP PHASE (Wait before spawning) ---
            float elapsed = 0f;
            while (elapsed < windupTime)
            {
                if (target == null || !inCombat) yield break;

                // Optional: Face target continuously during windup charge-up
                Vector3 dir = (target.position - transform.position).normalized;
                if (dir != Vector3.zero)
                {
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0f, 0f, angle);
                }

                elapsed += Time.deltaTime;
                yield return null;
            }

            // --- FIRE PROJECTILE ---
            if (target != null)
            {
                PetProjectiles.inst.SpawnDragonProjectile(transform.position, target.position);
            }

            // --- RECOVERY PHASE (Wait after spawning before next attack) ---
            elapsed = 0f;
            while (elapsed < recoveryTime)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        inCombat = false;
        attackRoutine = null;
    }


    #region Subscribe
    private void OnEnable()
    {
        RoomManager.OnRoomChanged += HandleRoomChanged;
    }

    private void OnDisable()
    {
        RoomManager.OnRoomChanged -= HandleRoomChanged;
        StopAttackRoutine();
    }
    #endregion
}