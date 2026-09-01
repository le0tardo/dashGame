using UnityEngine;

public class EnemyShatter : MonoBehaviour
{
    [SerializeField] private Rigidbody[] rbs;
    [SerializeField] private GameObject[] shards;
    [SerializeField] AudioClip shatterSound;

    [Header("Shrink & Lifetime")]
    [SerializeField] private float shrinkDuration = 1f;
    private float shrinkTimer = 1f;
    private bool isShattered = false;

    // Cache local start transforms so shards snap back perfectly
    private Vector3[] initialLocalPositions;
    private Quaternion[] initialLocalRotations;

    private void Awake()
    {
        // 1. Cache starting positions & rotations relative to the parent container
        initialLocalPositions = new Vector3[shards.Length];
        initialLocalRotations = new Quaternion[shards.Length];

        for (int i = 0; i < shards.Length; i++)
        {
            if (shards[i] == null) continue;
            initialLocalPositions[i] = shards[i].transform.localPosition;
            initialLocalRotations[i] = shards[i].transform.localRotation;
        }
    }

    public void Shatter(Vector3 shatterPosition)
    {
        transform.position = shatterPosition;
        shrinkTimer = 1f;
        isShattered = true;

        Vector3 playerVelocity = LevelManager.inst.playerMove.currentVelocity;

        for (int i = 0; i < rbs.Length; i++)
        {
            if (rbs[i] == null) continue;

            // Enable physics on active explosion
            rbs[i].isKinematic = false;
            rbs[i].linearVelocity = Vector3.zero;
            rbs[i].angularVelocity = Vector3.zero;

            rbs[i].AddForceAtPosition(playerVelocity, shatterPosition, ForceMode.Impulse);
        }

        gameObject.SetActive(true);
        if (shatterSound != null) AudioManager.inst.PlayCustomSound(shatterSound,1f);
    }

    private void Update()
    {
        if (!isShattered) return;

        if (shrinkTimer > 0f)
        {
            shrinkTimer -= Time.deltaTime / shrinkDuration;
            Vector3 currentScale = Vector3.one * Mathf.Clamp01(shrinkTimer);

            foreach (var shard in shards)
            {
                if (shard == null) continue;
                shard.transform.localScale = currentScale;
            }
        }
        else
        {
            ResetShatter();
        }
    }

    public void ResetShatter()
    {
        isShattered = false;

        // Reset all shards back to their saved local positions, rotations, and full scale
        for (int i = 0; i < shards.Length; i++)
        {
            if (shards[i] == null) continue;

            // 1. Reset Physics Body
            if (rbs[i] != null)
            {
                rbs[i].linearVelocity = Vector3.zero;
                rbs[i].angularVelocity = Vector3.zero;
                rbs[i].isKinematic = true; // Freeze physics while sitting in pool
            }

            // 2. Reset Transforms
            rbs[i].gameObject.transform.localPosition = initialLocalPositions[i];
            rbs[i].gameObject.transform.localRotation = initialLocalRotations[i];
            shards[i].transform.localScale = Vector3.one;
        }

        // Disable gameobject so the Object Pool knows it's available for reuse
        this.gameObject.transform.localPosition = Vector3.zero;
        gameObject.SetActive(false);
    }
}