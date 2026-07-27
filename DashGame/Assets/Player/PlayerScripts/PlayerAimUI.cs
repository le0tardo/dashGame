using UnityEngine;
using UnityEngine.EventSystems; // Required for UI drag interfaces!

public class PlayerAimUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Launch Stats")]
    [SerializeField] private float maxDragDistance = 10f;
    [SerializeField] private float speedMultiplier = 3f;
    [SerializeField] private float maxSpeed = 20f;

    [Header("Aim Arrow")]
    [SerializeField] private SpriteRenderer arrowSprite;
    [SerializeField] private float maxArrowLength;

    [SerializeField] private bool staminaAfford = true;

    [Header("References")]
    [SerializeField] private PlayerMove moveScript;
    private Camera mainCamera;

    private Vector2 dragStartScreenPos;
    private bool isDragging = false;

    void Start()
    {
        mainCamera = Camera.main;
        if (moveScript == null)
        {
            var player = FindAnyObjectByType<PlayerMove>();
            if (player != null) moveScript = player;
        }

        maxArrowLength = maxDragDistance;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (moveScript != null && !isDragging)
        {
            isDragging = true;
            dragStartScreenPos = eventData.position;
            LevelManager.inst.SlowDownTime();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        // Screen space delta vector
        Vector2 currentScreenPos = eventData.position;
        Vector2 screenDragVector = currentScreenPos - dragStartScreenPos;

        // Convert 2D screen swipe vector into a 3D horizontal world vector (X, Z)
        Vector3 dragVector = ConvertScreenVectorToWorldVector(screenDragVector);

        Vector3 launchDirection = -dragVector.normalized;
        float dragDistance = dragVector.magnitude;

        AimArrow(launchDirection, dragDistance);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isDragging) return;
        isDragging = false;

        Vector2 dragEndScreenPos = eventData.position;
        Vector2 screenDragVector = dragEndScreenPos - dragStartScreenPos;

        Vector3 dragVector = ConvertScreenVectorToWorldVector(screenDragVector);

        // Invert direction
        Vector3 launchDirection = -dragVector.normalized;
        //90 degree offset for UI aims.
        launchDirection = Quaternion.Euler(0f, -90f, 0f) * launchDirection;

        // Launch math...
        float dragDistance = Mathf.Min(dragVector.magnitude, maxDragDistance);
        float launchSpeed = dragDistance * speedMultiplier;
        launchSpeed = Mathf.Min(launchSpeed, maxSpeed);
        ResetArrow();

        if (launchSpeed < 10)
        {
            launchSpeed = 10;
        }

        staminaAfford = Mathf.Floor(launchSpeed / 10) <= LevelManager.inst.stamina;

        if (staminaAfford)
        {
            LevelManager.inst.UseStamina(Mathf.Floor(launchSpeed / 10));
            if (moveScript != null) moveScript.Launch(launchDirection, launchSpeed);
        }
        else
        {
            float remainderStamina = LevelManager.inst.stamina;
            float penalty = 5f;
            remainderStamina *= penalty;
            LevelManager.inst.UseStamina(Mathf.Floor(remainderStamina));
            if (moveScript != null) moveScript.Launch(launchDirection, remainderStamina);

            print("launch speed = stamina = " + remainderStamina);
            CameraShake.inst.Shake(0.1f, 1f);
        }

        LevelManager.inst.ResetTime();
    }

    // -------------------------------------------------------------
    // Helper Methods
    // -------------------------------------------------------------

    private Vector3 ConvertScreenVectorToWorldVector(Vector2 screenVector)
    {
        // Normalizes screen pixels into a usable world drag scale (dividing by a factor like 50 
        // keeps drag distance feeling consistent across different phone resolutions)
        float sensitivity = 50f;
        Vector3 worldVector = new Vector3(screenVector.x, 0f, screenVector.y) / sensitivity;

        return worldVector;
    }

    private void AimArrow(Vector3 launchDirection, float dragDistance)
    {
        if (moveScript == null) return;

        if (launchDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(launchDirection.x, launchDirection.z) * Mathf.Rad2Deg;
            angle = angle - 90;

            // Rotate the arrow
            arrowSprite.transform.rotation = Quaternion.Euler(90f, angle, 0f);

            // Rotate player object graphics directly
            moveScript.transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        float currentPercentage = Mathf.Clamp01(dragDistance / maxDragDistance);
        float targetLength = currentPercentage * maxArrowLength;
        arrowSprite.size = new Vector2(arrowSprite.size.x, targetLength);

        bool canAffordCurrentLaunch = dragDistance <= LevelManager.inst.stamina;
        arrowSprite.color = canAffordCurrentLaunch ? Color.green : Color.red;
    }

    private void ResetArrow()
    {
        arrowSprite.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        arrowSprite.size = new Vector2(2f, 2f);
        arrowSprite.color = Color.white;
    }
}