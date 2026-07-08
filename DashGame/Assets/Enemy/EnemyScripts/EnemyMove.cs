using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    [Header("Movement & Bounce")]
    [SerializeField] private float stopThreshold = 0.1f;
    [SerializeField] private float deceleration = 2f;
    [SerializeField] private float bounciness = 0.85f;
    [SerializeField] private float fallSpeed = 20;
    [SerializeField] private LayerMask wallLayers;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private float homingSpeed = 3f;
    private Transform playerTransform;
    private PlayerSlots playerSlots;
    private Vector3 targetSlotPosition;

    private Vector3 currentVelocity;
    private CapsuleCollider capsuleCollider;
    private NavMeshAgent agent;
    [SerializeField] public bool isBouncing = false;
    [SerializeField] public bool isFalling=false;

    EnemyCombat combat;

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        agent = GetComponent<NavMeshAgent>();
        combat= GetComponent<EnemyCombat>();

        if (agent != null)
        {
            agent.speed = homingSpeed;
            agent.acceleration = 30f;
            agent.angularSpeed = 360f;
            agent.acceleration = 1000;
            agent.avoidancePriority=Random.Range(1, 100);
        }

        PlayerMove player = Object.FindFirstObjectByType<PlayerMove>();
        if (player != null)
        {
            playerTransform = player.transform;
            playerSlots = player.GetComponent<PlayerSlots>();
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        if (isBouncing)
        {
            // pool ball mode
            currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);
            ExecuteMoveAndBounce();

            if (currentVelocity.magnitude <= stopThreshold)
            {
                currentVelocity = Vector3.zero;
                isBouncing = false;

                // Safety snap navmesh agent
                if (agent != null && !isFalling)
                {
                    agent.Warp(transform.position);
                    agent.enabled = true;
                }
            }
        }
        else
        {
            // zombie mode
            if (playerSlots != null && agent != null && agent.enabled)
            {
                targetSlotPosition = playerSlots.ReserveSlot(gameObject, out bool success);
                if(agent.isOnNavMesh)agent.SetDestination(targetSlotPosition);
            }
        }
    }

    public void GetHit(Vector3 incomingVelocity)
    {
        if (agent != null)
        {
            agent.enabled = false;
        }

        currentVelocity = incomingVelocity;
        isBouncing = true;

        float bounceDamage = Mathf.Floor(currentVelocity.magnitude/10);
        combat.TakeDamage(bounceDamage);

        if (playerSlots != null)
        {
            playerSlots.ReleaseSlot(gameObject);
        }
    }

    private void ExecuteMoveAndBounce()
    {
        Vector3 frameMovement = currentVelocity * Time.deltaTime;
        float distanceThisFrame = frameMovement.magnitude;
        Vector3 directionThisFrame = frameMovement.normalized;

        if (directionThisFrame != Vector3.zero)
        {
            float rotationSpeed = 15f;
            Quaternion targetRotation = Quaternion.LookRotation(directionThisFrame, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // calculate capsule cast
        float radius = 0.5f;
        float height = 2f;
        if (capsuleCollider != null)
        {
            radius = capsuleCollider.radius * transform.lossyScale.x;
            height = capsuleCollider.height * transform.lossyScale.y;
        }

        Vector3 pointBottom = transform.position + Vector3.up * (radius - (height / 2f));
        Vector3 pointTop = transform.position + Vector3.up * ((height / 2f) - radius);

        LayerMask combinedLayers = wallLayers | enemyLayers;
        if (Physics.CapsuleCast(pointBottom, pointTop, radius, directionThisFrame, out RaycastHit hit, distanceThisFrame, combinedLayers))
        {
            if (hit.collider == capsuleCollider)
            {
                transform.position += frameMovement;
                return;
            }

            transform.position += directionThisFrame * Mathf.Max(0, hit.distance - 0.01f);

            if ((enemyLayers.value & (1 << hit.collider.gameObject.layer)) > 0 && isBouncing)
            {
                if (hit.collider.gameObject.TryGetComponent<EnemyMove>(out EnemyMove otherEnemy))
                {
                    otherEnemy.GetHit(currentVelocity * 0.8f);
                }
            }

            currentVelocity = Vector3.Reflect(currentVelocity, hit.normal) * bounciness;
            currentVelocity.y = 0;
        }
        else
        {
            transform.position += frameMovement;
        }
    }

    public void Fall(Vector3 holePosition)
    {
        if (isFalling) return;//no double fall
        if (agent != null){agent.enabled = false;}
        isFalling = true;
        transform.position = holePosition;
        currentVelocity = Vector3.zero;
        StartCoroutine(DropDown());
    }

    private System.Collections.IEnumerator DropDown()
    {
        while (transform.position.y > -25f)
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            yield return null;
        }
        LevelManager.inst.AddScore(combat.health*2);
        Destroy(this.gameObject);
    }
}