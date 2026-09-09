using UnityEngine;
using System.Collections;
public class SpikeTrapBehaviour : MonoBehaviour
{

    [SerializeField] float damage = 1f;
    [SerializeField] float enemyDamage = 10f;
    [SerializeField] bool moving;
    [SerializeField] BoxCollider boxCollider;
    [SerializeField] AudioClip stab;
    [SerializeField] bool shouldStab;

    [Header("Animation")]
    [SerializeField] Transform spikes;
    [SerializeField] private Vector3 retractedLocalPos = new Vector3(0f, -1.33f, 0f);
    [SerializeField] private Vector3 extendedLocalPos = new Vector3(0f, -1f, 0f);
    [SerializeField] private AnimationCurve cycleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private float cycleDuration = 2.5f;
    [SerializeField] float hurtThresholdOn=0.1f;
    [SerializeField] float hurtThresholdOff=0.6f;


    private Coroutine loopRoutine;

    private void OnEnable()
    {
       if(moving) loopRoutine = StartCoroutine(LoopingTrapRoutine());
    }

    private void OnDisable()
    {
        if (loopRoutine != null)
        {
            StopCoroutine(loopRoutine);
            loopRoutine = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats=other.GetComponent<PlayerStats>();
            if (playerStats != null )
            {
                playerStats.TakeDamage(damage,transform.position);
            }
            AudioManager.inst.PlayCustomSound(stab,0.33f);
            CameraShake.inst.Shake(0.1f, 2f);
        }

        if (other.CompareTag("Enemy"))
        {
            EnemyCombat enemy = other.GetComponent<EnemyCombat>();
            if (enemy != null)
            {
                enemy.TakeDamage(enemyDamage);
            }
        }
    }

    private IEnumerator LoopingTrapRoutine()
    {
        while (true)
        {
            float elapsedTime = 0f;

            while (elapsedTime < cycleDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / cycleDuration;

                // Evaluate position anywhere along the single curve
                float curveValue = cycleCurve.Evaluate(t);
                spikes.localPosition = Vector3.LerpUnclamped(retractedLocalPos, extendedLocalPos, curveValue);

                bool shouldBeActive = (t >= hurtThresholdOn && t <= hurtThresholdOff);

                // Only call the engine if the state changed (saves CPU)
                if (boxCollider != null && boxCollider.enabled != shouldBeActive)
                {
                    boxCollider.enabled = shouldBeActive;
                    shouldStab = shouldBeActive;
                }

                yield return null;
            }

            // Ensure exact resting baseline at cycle completion
            spikes.localPosition = retractedLocalPos;
        }
    }

}
