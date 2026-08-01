using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Move & Bounce")]
    [SerializeField] private float stopThreshold = 0.1f;
    [SerializeField] private float deceleration = 2f;
    [SerializeField] private float bounciness = 0.85f;
    [SerializeField] private LayerMask hitLayer;
    [SerializeField] private float fallSpeed = 30f;
    [Header("Jump")]
    [SerializeField] private float jumpHeight = 4f;
    [SerializeField] private float jumpDuration = 0.6f; // Snappy airtime
    private bool isJumping = false;
    private float jumpYOffset = 0f;
    [SerializeField] float ground_y;

    public Vector3 currentVelocity=Vector3.zero;
    public float maxVelocity=50;
    private CapsuleCollider capsuleCollider;
    float playerRadius;
    private SphereCollider mouseCollider;
    private Rigidbody rb;

    public bool isMoving { get; private set; } = false;
    public bool isFalling { get; private set; } = false;

    [SerializeField] private float knockbackDuration = 0.2f;
    private bool isKnockedBack = false;

    private PlayerStats playerStats;
    [SerializeField] private PlayerAimUI playerAim;
    [SerializeField] Animator playerAnimator;

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        playerRadius=capsuleCollider.radius;
        mouseCollider = GetComponent<SphereCollider>();
        playerStats = GetComponent<PlayerStats>();
        if(playerAim==null)playerAim = GetComponent<PlayerAimUI>();
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
        currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);

        // Stop check
        if (currentVelocity.magnitude <= stopThreshold)
        {
            currentVelocity = Vector3.zero;
            isMoving = false;

            if (playerAnimator != null)
            {
                playerAnimator.SetTrigger("idle");
            }
            return;
        }

        // flat frame movement vectors
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

        //clean capsule dimensions dynamically //only if I dont have a capsule collider?
        float radius = (capsuleCollider != null) ? capsuleCollider.radius * transform.lossyScale.x : 0.5f;
        float height = (capsuleCollider != null) ? capsuleCollider.height * transform.lossyScale.y : 2f;
        Vector3 pointBottom = transform.position + Vector3.up * (radius - (height / 2f));
        Vector3 pointTop = transform.position + Vector3.up * ((height / 2f) - radius);

        //check layer mask
        if (Physics.CapsuleCast(pointBottom, pointTop, radius, directionThisFrame, out RaycastHit hit, distanceThisFrame,hitLayer))
        {
            // move to point of contact
            transform.position += directionThisFrame * Mathf.Max(0, hit.distance - 0.01f);
            float impactPower = currentVelocity.magnitude;
            float currentBounciness = bounciness;

            // check interface
            if (hit.collider.TryGetComponent<IHittable>(out IHittable hittable))
            {
                currentBounciness = hittable.hitBounce;
                hittable.OnHit(hit.point, impactPower); 

                //bounce on hittable
                currentVelocity = Vector3.Reflect(currentVelocity, hit.normal) * currentBounciness;
                currentVelocity.y = 0f;
                currentVelocity = Vector3.ClampMagnitude(currentVelocity, maxVelocity);
            }
            else
            {
                //bounce on wall
                currentVelocity = Vector3.Reflect(currentVelocity, hit.normal) * currentBounciness;
                currentVelocity.y = 0f;
                currentVelocity = Vector3.ClampMagnitude(currentVelocity, maxVelocity);

                //default wall feedback
                Vector3 fxPos = (hit.point + transform.position) / 2;
                HitFxManager.inst.HitFX1(fxPos, currentVelocity.magnitude / 10);
                CameraShake.inst.Shake(0.1f, 1f);
                AudioManager.inst.PlayImpactSound(currentVelocity.magnitude);
            }


        }
        else
        {
            //no collision, move forward
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
    public void StartKnockBack(Vector3 hitPos, float distance)
    {
        if (isKnockedBack || isFalling) return;

        // direction away from the hit
        Vector3 pushDirection = transform.position - hitPos;
        pushDirection.y = 0f; // Keep the calculation on a flat horizontal plane

        // fallback vector
        if (pushDirection == Vector3.zero)
        {
            pushDirection = -transform.forward;
        }

        Vector3 origin = transform.position;
        Vector3 direction = pushDirection.normalized;

        // find target destination
        Vector3 idealTargetPosition = origin + (direction * distance);
        Vector3 finalTargetPosition = idealTargetPosition;

        if (Physics.SphereCast(origin, playerRadius, direction, out RaycastHit hit, distance, hitLayer))
        {
            float safeDistance = hit.distance - 0.02f;
            safeDistance = Mathf.Max(0f, safeDistance);

            finalTargetPosition = origin + (direction * safeDistance);
        }
        StartCoroutine(KnockbackRoutine(finalTargetPosition));
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
        while (transform.position.y > (-10f))
        {   
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            yield return null;
        }
        LevelManager.inst.ChangeHealth(-10);
        RespawnPlayer();
    }
    void RespawnPlayer()
    {
        isFalling=false;
        currentVelocity = Vector3.zero;
        transform.position=RoomManager.inst.currentRoom.transform.position;
    }
}