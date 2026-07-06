using UnityEngine;

public class PlayerSlots : MonoBehaviour
{
    [System.Serializable]
    public struct Slot
    {
        public Vector3 localPosition;
        public GameObject occupier;
    }

    [Header("Ring Configuration")]
    [SerializeField] private int totalSlots = 8;
    [SerializeField] private float ringRadius = 2.5f;

    private Slot[] slots;

    void Awake()
    {
        InitializeSlots();
    }

    void Update()
    {
        UpdateSlotPositions();
    }

    private void InitializeSlots()
    {
        slots = new Slot[totalSlots];
        UpdateSlotPositions(); // Calculate initial placement
    }

    // Keep the local slot positions locked in a perfect circle relative to the player
    private void UpdateSlotPositions()
    {
        if (slots == null || slots.Length != totalSlots) slots = new Slot[totalSlots];

        float angleStep = 360f / totalSlots;
        for (int i = 0; i < totalSlots; i++)
        {
            float angleRad = i * angleStep * Mathf.Deg2Rad;
            slots[i].localPosition = new Vector3(Mathf.Cos(angleRad), 0f, Mathf.Sin(angleRad)) * ringRadius;
        }
    }

    // UPGRADED: Hand out the NEAREST open slot to the requesting enemy
    public Vector3 ReserveSlot(GameObject enemy, out bool success)
    {
        // 1. If this enemy already owns a chair, just give them its updated world position
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].occupier == enemy)
            {
                success = true;
                return transform.position + slots[i].localPosition;
            }
        }

        // 2. Proximity Search: Find the closest vacant slot
        int bestSlotIndex = -1;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].occupier == null) // Checking null directly!
            {
                Vector3 slotWorldPos = transform.position + slots[i].localPosition;
                float distanceToEnemy = Vector3.Distance(slotWorldPos, enemy.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    bestSlotIndex = i;
                }
            }
        }

        // 3. If we found a winner, book it!
        if (bestSlotIndex != -1)
        {
            slots[bestSlotIndex].occupier = enemy;
            success = true;
            return transform.position + slots[bestSlotIndex].localPosition;
        }

        // 4. Ring is completely full
        success = false;
        return transform.position;
    }

    public void ReleaseSlot(GameObject enemy)
    {
        if (slots == null) return;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].occupier == enemy)
            {
                slots[i].occupier = null;
                return;
            }
        }
    }

    // NEW: Draw Debug Gizmos in the Unity Editor Scene View
    private void OnDrawGizmos()
    {
        // If the game isn't running yet, draw a preview circle of white spheres
        if (!Application.isPlaying)
        {
            Gizmos.color = Color.white;
            float previewAngleStep = 360f / totalSlots;
            for (int i = 0; i < totalSlots; i++)
            {
                float angleRad = i * previewAngleStep * Mathf.Deg2Rad;
                Vector3 localPos = new Vector3(Mathf.Cos(angleRad), 0f, Mathf.Sin(angleRad)) * ringRadius;
                Gizmos.DrawWireSphere(transform.position + localPos, 0.2f);
            }
            return;
        }

        // During play mode, color-code based on occupancy!
        if (slots == null) return;

        for (int i = 0; i < slots.Length; i++)
        {
            Vector3 worldPos = transform.position + slots[i].localPosition;

            if (slots[i].occupier == null)
            {
                Gizmos.color = Color.green; // Vacant chair
                Gizmos.DrawSphere(worldPos, 0.25f);
            }
            else
            {
                Gizmos.color = Color.red; // Taken chair
                Gizmos.DrawSphere(worldPos, 0.25f);
                // Draw a handy thin line from the slot to its zombie occupier
                Gizmos.DrawLine(worldPos, slots[i].occupier.transform.position);
            }
        }
    }
}