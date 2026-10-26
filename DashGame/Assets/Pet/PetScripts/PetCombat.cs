using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetCombat : MonoBehaviour
{
    [SerializeField] private PetObject pet;
    [SerializeField] private Transform target = null;
    [SerializeField] private bool inCombat = false;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float baseAttackSpeed = 3f;

    private RoomBehaviour currentRoom;
    private Coroutine attackRoutine;

    private void Start()
    {
        if (pet != null)
        {
            attackDamage = pet.petAttackPower;
            attackSpeed = pet.petAttackSpeed / 10f;

            print("read attack speed: "+ pet.petAttackSpeed+". modified attackSpeed: " + attackSpeed);
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
            if (currentRoom.enemies[i] != null && currentRoom.enemies[i].gameObject.activeInHierarchy)
            {
                return currentRoom.enemies[i].transform;
            }
        }
        return null;
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
            Vector3 direction = (target.position - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }

            PetProjectiles.inst.SpawnDragonProjectile(transform.position,target.position);

            //float attackDelay = baseAttackSpeed / Mathf.Max(0.1f, attackSpeed);

            float speedMultiplier = attackSpeed;

            // 2. Prevent division by zero or negative values if stat drops low
            speedMultiplier = Mathf.Max(0.1f, speedMultiplier);

            // 3. Calculate delay (Base 3s divided by multiplier)
            float attackDelay = baseAttackSpeed / speedMultiplier;

            // 4. Clamp to a hard minimum delay limit (e.g., 0.1s minimum delay = max 10 attacks/sec)
            attackDelay = Mathf.Max(0.1f, attackDelay);

            print("pet attack delay: " + attackDelay);

            float elapsed = 0f;
            while (elapsed < attackDelay)
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