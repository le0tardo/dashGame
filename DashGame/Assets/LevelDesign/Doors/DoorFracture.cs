using UnityEngine;

public class DoorFracture : MonoBehaviour
{
    [SerializeField] Rigidbody[] shardsRb;
    [SerializeField] GameObject[] shardGfx;
    PlayerMove player;

    [SerializeField] bool shrinkBool=false;
    [SerializeField] float shrinkFloat=1f;
    [SerializeField] float shrinkRate = 0.1f;

    [SerializeField] private float customGravity = 25f;

    private void Awake()
    {
        player = FindAnyObjectByType<PlayerMove>();
    }
    private void Update()
    {
        if (shrinkBool)
        {
            Shrink();
        }
    }
    private void FixedUpdate()
    {
        ApplyCustomGravity();
    }

    public void Shatter()
    {
        foreach (Rigidbody shard in shardsRb)
        {
            if (shard == null) continue;

            Vector3 pushDir = (shard.transform.position - player.transform.position).normalized;
            pushDir.y += 2 * 0.1f;
            float shatterForce=player.currentVelocity.magnitude;
            shard.AddForce(pushDir * shatterForce, ForceMode.Impulse);
        }
        shrinkBool = true;
    }

    void Shrink()
    {
        shrinkFloat -= shrinkRate * Time.deltaTime;
        foreach (var shard in shardGfx)
        {
            shard.gameObject.transform.localScale = new Vector3(shrinkFloat, shrinkFloat, shrinkFloat);
        }

        if (shrinkFloat <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void ApplyCustomGravity()
    {
        Vector3 gravityForce = Vector3.down * customGravity;

        foreach (Rigidbody shard in shardsRb)
        {
            if (shard == null || shard.isKinematic) continue;

            // ForceMode.Acceleration applies force ignoring mass (behaves just like real gravity)
            shard.AddForce(gravityForce, ForceMode.Acceleration);
        }
    }
}
