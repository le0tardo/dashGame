using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [Header("Launch Stats")]
    [SerializeField] private float maxDragDistance = 10f;
    [SerializeField] private float speedMultiplier = 3f;
    [SerializeField] private float maxSpeed = 20f;

    [Header("Aim Arrow")]
    [SerializeField] private SpriteRenderer arrowSprite;
    [SerializeField] private float maxArrowLength;

    private PlayerMove moveScript;
    private Camera mainCamera;
    private Vector3 dragStartPosition;
    private bool isDragging = false;

    void Start()
    {
        mainCamera = Camera.main;
        moveScript = GetComponent<PlayerMove>();
        maxArrowLength=maxDragDistance;
    }

    void OnMouseDown()
    {
        if (moveScript != null && !isDragging)
        {
            isDragging = true;
            dragStartPosition = GetMouseWorldPosition();
            LevelManager.inst.SlowDownTime();
        }
    }
    void OnMouseDrag()
    {
        if (!isDragging) return;

        Vector3 currentMousePos = GetMouseWorldPosition();
        Vector3 dragVector = currentMousePos - dragStartPosition;

        Vector3 launchDirection = -dragVector.normalized;
        float dragDistance = dragVector.magnitude;

        AimArrow(launchDirection, dragDistance);
    }

    void OnMouseUp()
    {
        if (!isDragging) return;
        isDragging = false;

        Vector3 dragEndPosition = GetMouseWorldPosition();
        Vector3 dragVector = dragEndPosition - dragStartPosition;

        // invert direction
        Vector3 launchDirection = -dragVector.normalized;

        // launch math...
        float dragDistance = Mathf.Min(dragVector.magnitude, maxDragDistance);
        float launchSpeed = dragDistance * speedMultiplier;
        launchSpeed = Mathf.Min(launchSpeed, maxSpeed);
        ResetArrow();

        // Launch!
        if(moveScript!=null)moveScript.Launch(launchDirection, launchSpeed);
        LevelManager.inst.ResetTime();
    }

    private void AimArrow(Vector3 launchDirection, float dragDistance)
    {
        //Rotation override
        if (launchDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(launchDirection.x, launchDirection.z) * Mathf.Rad2Deg;
            arrowSprite.transform.rotation = Quaternion.Euler(90f, angle, 0f);

            //rotate player gfx too?
            this.gameObject.transform.rotation = Quaternion.Euler(0, angle, 0);
        }
        //size
        float currentPercentage = Mathf.Clamp01(dragDistance / maxDragDistance);
        float targetLength = currentPercentage * maxArrowLength;
        arrowSprite.size = new Vector2(arrowSprite.size.x, targetLength);
    }
    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane horizontalPlane = new Plane(Vector3.up, transform.position);

        if (horizontalPlane.Raycast(ray, out float enter))
        {
            return ray.GetPoint(enter);
        }
        return transform.position;
    }

    private void ResetArrow()
    {
        arrowSprite.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        arrowSprite.size = new Vector2(2,2);
    }
}