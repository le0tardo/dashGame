using UnityEngine;
using System.Collections;
public class SpikeBatBehaviour : MonoBehaviour, IHittable
{
    [SerializeField] float damage = 1f;
    [SerializeField] float bounce = 1.5f;
    public float hitBounce => bounce;
    [SerializeField] AudioClip[] ting;

    [Header("wobble animation")]
    [SerializeField] AnimationCurve wobbleCurve;
    [SerializeField] GameObject wobbleObject;
    [SerializeField] private float wobbleAngleMax = 15f;
    [SerializeField] private float duration = 0.5f;
    private Coroutine wobbleRoutine;
    private Quaternion initialLocalRotation;

    private void Awake()
    {
        initialLocalRotation = transform.localRotation;
    }
    public void OnHit(Vector3 hitPosition, float power)
    {

        PlayerStats player = LevelManager.inst.playerStats;
        PlayerMove playerMove = LevelManager.inst.playerMove;

        float dealtDamage = damage * (playerMove.currentVelocity.magnitude/10);
        dealtDamage= Mathf.Floor(dealtDamage);

        player.SubtractHealth(dealtDamage); //no knockback coroutine

        Vector3 fxPos = (transform.position + hitPosition) / 2;
        HitFxManager.inst.HitFX1(fxPos, power / 10);
        CameraShake.inst.Shake(0.1f, 2f);

        if (ting != null)
        {
            int r = Random.Range(0, ting.Length);
            float vol = power / 100;
            vol = vol * 2;
            Mathf.Clamp(vol, 0.2f, 1f);
            AudioManager.inst.PlayCustomSound(ting[r], vol);
        }
        else
        {
            AudioManager.inst.PlayImpactSound(power);
        }

        TriggerWobble();
    }

    public void TriggerWobble()
    {
        if (wobbleRoutine != null)
        {
            StopCoroutine(wobbleRoutine);
            transform.localRotation = initialLocalRotation;
        }
        wobbleRoutine = StartCoroutine(WobbleRoutine());
    }

    private IEnumerator WobbleRoutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // 1. Evaluate curve value
            float curveValue = wobbleCurve.Evaluate(t);

            // 2. Calculate local X-axis rotation offset
            float angle = curveValue * wobbleAngleMax;
            Quaternion xRotation = Quaternion.AngleAxis(angle, Vector3.right); // Vector3.right = Local X Axis

            // 3. Apply offset relative to the original local rotation
            wobbleObject.transform.localRotation = initialLocalRotation * xRotation;

            yield return null;
        }

        // Snap back to exact starting rotation when finished
        wobbleObject.transform.localRotation = initialLocalRotation;
        wobbleRoutine = null;
    }
}
