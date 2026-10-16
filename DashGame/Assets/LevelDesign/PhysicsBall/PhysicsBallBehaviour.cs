using UnityEngine;

public class PhysicsBallBehaviour : MonoBehaviour, IHittable
{
    [SerializeField] float bounce = 0.85f;
    public float hitBounce => bounce;

    [SerializeField] AudioClip hitSound;

    [SerializeField] Transform player;

    // Store Vector3 and Quaternion values instead of a Transform reference
    private Vector3 startingPosition;
    private Quaternion startingRotation;
    private Vector3 startingScale;

    Rigidbody rb;

    [SerializeField] BombBallBehaviour bomb;

    [SerializeField] float fallAcceleration = 15f;
    [SerializeField] float maxFallSpeed = 20f;
    float fallSpeed = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (LevelManager.inst != null && LevelManager.inst.playerMove != null)
        {
            player = LevelManager.inst.playerMove.transform;
        }

        // Cache the actual position and rotation values at start
        startingPosition = transform.position;
        startingRotation = transform.rotation;
        startingScale = transform.localScale;
    }

    public void OnHit(Vector3 hitPosition, float power)
    {
        if (LevelManager.inst != null && LevelManager.inst.playerMove != null)
        {
            player = LevelManager.inst.playerMove.transform;
        }

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Vector3 pushDirection = transform.position - player.position;
        pushDirection.y = 0f;
        pushDirection = pushDirection.normalized;
        float powerMultiplier = 4f;
        rb.AddForce(pushDirection * (power * powerMultiplier), ForceMode.Impulse);

        Vector3 fxPos = (transform.position + hitPosition) / 2;
        HitFxManager.inst.HitFX1(fxPos, power / 10);
        CameraShake.inst.Shake(0.1f, 1f);

        if (hitSound != null) AudioManager.inst.PlayCustomSound(hitSound, power / 50);

        if (bomb != null) bomb.Ignite();
    }

    void Update()
    {
        if (rb.isKinematic)
        {
            fallSpeed = Mathf.Min(fallSpeed + fallAcceleration * Time.deltaTime, maxFallSpeed);
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

       if (other.CompareTag("Hole"))
        {
            rb.isKinematic = true;
            rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
            Invoke(nameof(ResetBall), 2f);
        }
       
    }

    void ResetBall()
    {
        // 1. Reset velocities while still kinematic to prevent physics solver interference
        if (!rb.isKinematic)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // 2. Set physics position directly via Rigidbody
        rb.position = startingPosition;
        rb.rotation = startingRotation;
        transform.localScale = startingScale;

        // 3. Restore constraints and non-kinematic state

        rb.isKinematic = false;
        //rb.constraints |= RigidbodyConstraints.FreezePositionY; //move to delay

        fallSpeed = 0f;
        Physics.SyncTransforms();
        Invoke(nameof(FreezeY), 0.1f);
    }

    void FreezeY()
    {
        rb.constraints |= RigidbodyConstraints.FreezePositionY;
    }
}