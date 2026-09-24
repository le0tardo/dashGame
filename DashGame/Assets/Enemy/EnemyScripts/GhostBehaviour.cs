using UnityEngine;
using UnityEngine.AI;

public class GhostBehaviour : MonoBehaviour
{
    private enum ghostState { Turning, Charging, Dying }
    [SerializeField] private ghostState state;
    [SerializeField] private Transform target;
    [SerializeField] private Transform roomCentre;
    [SerializeField] private bool graveBound = true;
    [SerializeField] private GameObject grave;
    private string roomTag = "Room";
    [SerializeField] int xp;

    [Header("Move stats")]
    [SerializeField] private float turnSpeed = 1f;
    [SerializeField] private float initialChargeSpeed = 5f; // Added default base speed
    [SerializeField] private float chargeSpeed = 1f;
    [SerializeField] private float maxChargeSpeed = 25f;
    [SerializeField] private bool accelerate = true; // Default to true
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private LayerMask hitLayer;
    private float targetAngleThreshold = 5f;
    private Vector3 chargeDirection;

    [Header("Attack stats")]
    [SerializeField] private bool canHit = true;
    [SerializeField] private float damage = 1f;
    private bool dead = false;

    [Header("Graphics")]
    [SerializeField] private Animator anim;

    private void Start()
    {
        target = LevelManager.inst.playerMove.gameObject.transform;
        roomCentre = RoomManager.inst.currentRoom.transform;
        state = ghostState.Turning;
    }

    private void OnEnable()
    {
        // Reset state for object pooling reuse
        dead = false;
        canHit = true;
        accelerate = true;
        chargeSpeed = initialChargeSpeed;
        state = ghostState.Turning;
    }

    private void Update()
    {
        if (target == null) return;

        if (grave == null && graveBound)
        {
            state = ghostState.Dying;
        }

        switch (state)
        {
            case ghostState.Turning:
                Turn();
                break;

            case ghostState.Charging:
                Charge();
                break;

            case ghostState.Dying:
                Die();
                break;
        }
    }

    private void Turn()
    {
        bool isOverlapping = Physics.CheckSphere(transform.position, 1f, hitLayer);

        if (isOverlapping)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                roomCentre.position,
                25f * Time.deltaTime
            );
        }

        Vector3 targetDirection = (target.position - transform.position);
        targetDirection.y = 0f;

        if (targetDirection == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

        float angleToTarget = Quaternion.Angle(transform.rotation, targetRotation);

        if (angleToTarget <= targetAngleThreshold)
        {
            // --- STATE RESET FIX ---
            accelerate = true;
            chargeSpeed = initialChargeSpeed; // Ensure non-zero starting speed

            Vector3 exactTargetDir = (target.position - transform.position);
            exactTargetDir.y = 0f;
            chargeDirection = exactTargetDir.normalized;

            if (chargeDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(chargeDirection);
            }

            canHit = true;
            AudioManager.inst.PlaySwoosh(0.5f);
            anim.SetTrigger("charge");
            state = ghostState.Charging;
        }
    }

    private void Charge()
    {
        // 1. Calculate acceleration
        if (accelerate && chargeSpeed < maxChargeSpeed)
        {
            chargeSpeed += acceleration * Time.deltaTime;
        }

        // 2. Determine intended delta movement for this frame
        Vector3 currentPos = transform.position;
        Vector3 displacement = chargeDirection * (chargeSpeed * Time.deltaTime);
        Vector3 targetPos = currentPos + displacement;

        // Ensure raycast query stays flat on the horizontal plane
        Vector3 rayStart = currentPos;
        Vector3 rayEnd = targetPos;

        // 3. Probe the NavMesh surface ahead
        if (NavMesh.Raycast(rayStart, rayEnd, out NavMeshHit hit, NavMesh.AllAreas))
        {
            transform.position = new Vector3(hit.position.x, currentPos.y, hit.position.z);
            EndCharge();
        }
        else
        {
            transform.position = new Vector3(targetPos.x, currentPos.y, targetPos.z);
        }
    }

    private void EndCharge()
    {
        chargeSpeed = 0f;
        accelerate = false;
        anim.SetTrigger("stop");
        state = ghostState.Turning;
    }

    private void Die()
    {
        if (!dead)
        {
            dead = true;
            anim.SetTrigger("die");
            Invoke("ReturnToPool", 0.66f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(roomTag) && state == ghostState.Charging)
        {
            EndCharge();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canHit && state == ghostState.Charging)
        {
            if (other.TryGetComponent(out PlayerStats player))
            {
                player.TakeDamage(damage, transform.position);
            }
            canHit = false;
        }
    }

    private void ReturnToPool()
    {
        ShatterManager.inst.shatterGhost(transform.position);
        OrbPool.inst.SpawnOrbs(xp, transform.position);
        gameObject.SetActive(false);
    }
}