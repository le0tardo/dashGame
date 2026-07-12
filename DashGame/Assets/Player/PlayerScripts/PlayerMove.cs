using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Move & Bounce")]
    [SerializeField] private float stopThreshold = 0.1f;
    [SerializeField] private float deceleration = 2f;
    [SerializeField] private float bounciness = 0.85f;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask doorLayer;
    [SerializeField] private LayerMask hurtLayer;
    [SerializeField] private float fallSpeed = 30f;
    [Header("Jump")]
    [SerializeField] private float jumpHeight = 4f;
    [SerializeField] private float jumpDuration = 0.6f; // Snappy airtime
    private bool isJumping = false;
    private float jumpYOffset = 0f; // Stores our temporary height
    [SerializeField] float ground_y;

    private Vector3 currentVelocity;
    private CapsuleCollider capsuleCollider;
    private SphereCollider mouseCollider;
    private Rigidbody rb;

    public bool isMoving { get; private set; } = false;
    public bool isFalling { get; private set; } = false;

    [SerializeField] private float knockbackDuration = 0.2f;
    private bool isKnockedBack = false;

    private PlayerStats playerStats;
    private PlayerAim playerAim;

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        mouseCollider = GetComponent<SphereCollider>();
        playerStats = GetComponent<PlayerStats>();
        playerAim = GetComponent<PlayerAim>();
        rb= GetComponent<Rigidbody>();
        rb.isKinematic = true;

        ground_y=transform.position.y;
    }

    void Update()
    {
        if (isFalling) return;

        if (isMoving)
        {
            MoveAndBounce();
        }

        if (mouseCollider != null)
        {
            mouseCollider.enabled = !isMoving;
        }
    }

    public void Launch(Vector3 direction, float speed)
    {
        if (isKnockedBack || isFalling || isJumping) return;
        currentVelocity = direction * speed;
        transform.position = new Vector3(transform.position.x, ground_y, transform.position.z); //safety snap back to floor
        currentVelocity.y = 0; // Lock perfectly flat to the table surface
        isMoving = true;
    }

    private void MoveAndBounce()
    {
        // 1. Decelerate over time via friction
        currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);

        // Stop check
        if (currentVelocity.magnitude <= stopThreshold)
        {
            currentVelocity = Vector3.zero;
            isMoving = false;
            return;
        }

        // 2. Establish flat frame movement vectors
        Vector3 frameMovement = currentVelocity * Time.deltaTime;
        float distanceThisFrame = frameMovement.magnitude;
        Vector3 directionThisFrame = frameMovement.normalized;

        // face direction
        if (directionThisFrame != Vector3.zero)
        {
            float rotationSpeed = 15f;
            Quaternion targetRotation = Quaternion.LookRotation(directionThisFrame, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // 3. Setup clean capsule dimensions dynamically
        float radius = (capsuleCollider != null) ? capsuleCollider.radius * transform.lossyScale.x : 0.5f;
        float height = (capsuleCollider != null) ? capsuleCollider.height * transform.lossyScale.y : 2f;

        Vector3 pointBottom = transform.position + Vector3.up * (radius - (height / 2f));
        Vector3 pointTop = transform.position + Vector3.up * ((height / 2f) - radius);

        LayerMask combinedLayers = wallLayer | enemyLayer | doorLayer | hurtLayer;

        // 4. collision check
        if (Physics.CapsuleCast(pointBottom, pointTop, radius, directionThisFrame, out RaycastHit hit, distanceThisFrame, combinedLayers))
        {
            CheckHitLayer(combinedLayers);
            // Move forward to point of contact
            transform.position += directionThisFrame * Mathf.Max(0, hit.distance - 0.01f);

            if (CameraShake.inst != null)
            {
                CameraShake.inst.Shake(0.1f, (currentVelocity.magnitude / 100f));
            }

            // enemies
            if ((enemyLayer.value & (1 << hit.collider.gameObject.layer)) > 0)
            {
                //walking enemy
                if (hit.collider.gameObject.TryGetComponent<EnemyMove>(out EnemyMove enemy))
                {
                    enemy.GetHit(currentVelocity);

                    float juiceScale = Mathf.Floor(currentVelocity.magnitude / 10f);
                    HitFxManager.inst.HitFX1(hit.point, juiceScale);
                    LevelManager.inst.AddScore(juiceScale);
                }
                //enemy turret
                if(hit.collider.gameObject.TryGetComponent<TurretEnemy>(out TurretEnemy tenemy))
                {
                    print("player hit turret");
                    tenemy.GetHit(Mathf.Floor(currentVelocity.magnitude/10f));
                }
            }
            if ((doorLayer.value & (1 << hit.collider.gameObject.layer)) > 0)
            {
                print("player hit door!");
                DoorBehaviour door =hit.collider.gameObject.GetComponent<DoorBehaviour>();
                if (door != null)
                {
                    door.KnockOnDoor(Mathf.Floor(currentVelocity.magnitude / 10f));
                }
            }
            if ((hurtLayer.value & (1 << hit.collider.gameObject.layer)) > 0)
            {
                float hurtForce=(currentVelocity.magnitude/10);
                Vector3 hurtPos = hit.collider.gameObject.transform.position;
                playerStats.Hurt(1,hurtPos);
            }

                // bounce physics
                currentVelocity = Vector3.Reflect(currentVelocity, hit.normal) * bounciness;
                currentVelocity.y = 0;
        }
        else
        {
            // Safe path clear: advance normally
            transform.position += frameMovement;
        }
    }
    public void Jump()
    {
        // Safety gate: Don't jump if already jumping or falling down a hole
        if (isJumping || isFalling) return;

        StartCoroutine(JumpRoutine());
    }
    private System.Collections.IEnumerator JumpRoutine()
    {
        isJumping = true;
        float elapsedTime = 0f;
        float tableSurfaceY = transform.position.y - jumpYOffset;

        while (elapsedTime < jumpDuration)
        {

            elapsedTime += Time.unscaledDeltaTime;

            float percent = Mathf.Clamp01(elapsedTime / jumpDuration);

            float arcNormalized = 4f * percent * (1f - percent);

            jumpYOffset = arcNormalized * jumpHeight;

            Vector3 currentPos = transform.position;
            currentPos.y = tableSurfaceY + jumpYOffset;
            transform.position = currentPos;

            yield return null;
        }

        Vector3 finalPos = transform.position;
        finalPos.y = tableSurfaceY;
        transform.position = finalPos;

        jumpYOffset = 0f;
        isJumping = false;
    }
    void CheckHitLayer(LayerMask layer)
    {
        //print("Hit: " + layer.ToString()); //need to format af, forloops and stuff...
    }
    public void StartKnockBack(Vector3 hitPos, float distance)
    {
        if (isKnockedBack || isFalling) return;

        // calculate direction away from enemy
        Vector3 pushDirection = transform.position - hitPos;
        pushDirection.y = 0f;
        //fallback vector
        if (pushDirection == Vector3.zero)
        {
            pushDirection = -transform.forward;
        }

        // calculate destination point
        Vector3 targetPosition = transform.position + (pushDirection.normalized * distance);

        // start movement
        StartCoroutine(KnockbackRoutine(targetPosition));
    }

    private System.Collections.IEnumerator KnockbackRoutine(Vector3 targetPosition)
    {
        isKnockedBack = true;

        currentVelocity = Vector3.zero;
        isMoving = false;

        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < knockbackDuration)
        {
            elapsedTime += Time.deltaTime;
            float percent = elapsedTime / knockbackDuration;
            float curvedPercent = Mathf.SmoothStep(0f, 1f, percent);

            transform.position = Vector3.Lerp(startPosition, targetPosition, curvedPercent);
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, ground_y, transform.position.z); //safety snap back to floor
        isKnockedBack = false;
    }

    //Trigger colliders from pockets.
    public void Fall(Vector3 holePosition)
    {
        if (!isFalling)
        {
            isFalling = true;
            isMoving = false;
            currentVelocity = Vector3.zero;
            transform.position = new Vector3(holePosition.x, transform.position.y, holePosition.z);

            StartCoroutine(DropDown());
        }
    }

    private System.Collections.IEnumerator DropDown()
    {
        while (transform.position.y > -25f)
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            yield return null;
        }
    }
}