using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Move & Bounce")]
    [SerializeField] private float stopThreshold = 0.1f;
    [SerializeField] private float deceleration = 2f;
    [SerializeField] private float bounciness = 0.85f;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float fallSpeed = 30f;

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
        currentVelocity = direction * speed;
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

        // Face movement direction smoothly
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

        LayerMask combinedLayers = wallLayer | enemyLayer;

        // 4. Manual swept collision check
        if (Physics.CapsuleCast(pointBottom, pointTop, radius, directionThisFrame, out RaycastHit hit, distanceThisFrame, combinedLayers))
        {
            // Move forward right up to the point of contact
            transform.position += directionThisFrame * Mathf.Max(0, hit.distance - 0.01f);

            // Dynamic camera shake based on velocity intensity
            if (CameraShake.inst != null)
            {
                CameraShake.inst.Shake(0.1f, (currentVelocity.magnitude / 100f));
            }

            // Hit processing for enemies
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

            // Bounce physics: Reflect the vector cleanly off the wall/obstacle normal
            currentVelocity = Vector3.Reflect(currentVelocity, hit.normal) * bounciness;
            currentVelocity.y = 0;
        }
        else
        {
            // Safe path clear: advance normally
            transform.position += frameMovement;
        }
    }

    public void KnockBack(Vector3 hitPos, float hitForce)
    {
        // 1. Calculate the direction pushing AWAY from the impact source
        Vector3 pushDirection = transform.position - hitPos;

        // 2. Lock it perfectly flat to the table surface (Y = 0)
        pushDirection.y = 0f;

        // Safety check: If the hit position is somehow exactly on the player's center,
        // default to pushing them backward relative to where they are currently facing.
        if (pushDirection == Vector3.zero)
        {
            pushDirection = -transform.forward;
        }

        // 3. Inject this vector into your existing movement loop
        // This functions exactly like Launch(), triggering isMoving and applying deceleration!
        currentVelocity = pushDirection.normalized * hitForce*10;
        print("knockback player at direction: " + pushDirection.normalized + " with force: " + hitForce*10);
        isMoving = true;//idk??
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