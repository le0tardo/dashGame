using UnityEngine;
using System.Collections;
public class PickupOrbBehaviour : MonoBehaviour
{
    enum PickupType
    {
        Stamina,
        Health,
        XP
    }
    [SerializeField] PickupType pickupType;
    [SerializeField] float amount = 1f;

    [Header("Spawn")]
    [SerializeField] private AnimationCurve speedCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [SerializeField] private float minRadius = 1.5f;
    [SerializeField] private float maxRadius = 3f;
    [SerializeField] private float minMoveDuration = 0.5f;
    [SerializeField] private float maxMoveDuration = 1f;
    [SerializeField] bool pickupReady = false;
    float moveDuration;

    [Header("Pickup")]
    [SerializeField] Transform player;
    [SerializeField] private AnimationCurve floatSpeedCurve;// = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [SerializeField] private float pickupRadius = 1.5f;
    [SerializeField] private float floatDuration = 0.25f;

    private void Start()
    {

    }

    private void OnEnable()
    {
        moveDuration = Random.Range(minMoveDuration, maxMoveDuration);
        float randomDistance = Random.Range(minRadius, maxRadius);
        Vector2 randomCircle = Random.insideUnitCircle.normalized * randomDistance;
        Vector3 targetPos = transform.position + new Vector3(randomCircle.x, 0f, randomCircle.y);

        player = LevelManager.inst.playerMove.gameObject.transform;

        StartCoroutine(MoveRoutine(targetPos));
    }

    private void Update()
    {
        if (!pickupReady || player == null) return;

        float sqrDistance = (transform.position - player.position).sqrMagnitude;
        float targetSqrRadius = pickupRadius * pickupRadius;

        if (sqrDistance <= targetSqrRadius)
        {
            pickupReady = false; // Prevent multiple triggers
            StartCoroutine(FloatToPlayerRoutine());
        }
    }
    private IEnumerator MoveRoutine(Vector3 target)
    {
        Vector3 startPos = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveDuration;
            float curveT = speedCurve.Evaluate(t);
            transform.position = Vector3.Lerp(startPos, target, curveT);
            yield return null;
        }

        transform.position = target;
        pickupReady = true;
    }

    private IEnumerator FloatToPlayerRoutine()
    {
        Vector3 startPos = transform.position;

        // Calculate total duration once based on initial distance
        float initialDist = Vector3.Distance(startPos, player.position);
        float totalDuration = floatDuration + (initialDist / 10f);

        float elapsedTime = 0f;

        while (elapsedTime < totalDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / totalDuration;

            // Evaluate curve (e.g., goes down to -0.2 before curving up to 1.0)
            float curveT = floatSpeedCurve.Evaluate(t);

            // Vector3.LerpUnclamped allows curveT to go below 0 or above 1
            transform.position = Vector3.LerpUnclamped(startPos, player.position, curveT);

            yield return null;
        }

        transform.position = player.position;
        PickUp();
    }
    void PickUp()
    {
        switch (pickupType)
        {
            case PickupType.Stamina:
                LevelManager.inst.GetStamina(amount);
                AudioManager.inst.PlayStaminaPickup();
                break;

            case PickupType.Health:
                //comin soon
            break;

             case PickupType.XP:
                LevelManager.inst.GetXp(amount);
                AudioManager.inst.PlayPickupXP();
                HitFxManager.inst.PickupXpFx(player.position);
                break;
        }

        this.gameObject.SetActive(false);
    }

}
