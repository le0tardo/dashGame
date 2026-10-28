using UnityEngine;
using System.Collections;

public class PetProjectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed=10f;
    [SerializeField] private float maxLifetime = 3f;

    [SerializeField] Vector3 targetPosition = Vector3.zero;
    [SerializeField] PetCombat pet; //either interface or this reference...

    [SerializeField] AudioClip fireballSpawn;
    [SerializeField] AudioClip fireballHit;

    private Coroutine moveRoutine=null;

    public void FireProjectile(Vector3 _targetPosition)
    {
        if(pet==null)pet=FindAnyObjectByType<PetCombat>();

        targetPosition= new Vector3 (_targetPosition.x,transform.position.y,_targetPosition.z);

        Vector3 direction = (targetPosition - transform.position).normalized;


        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        if (moveRoutine != null) StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(MoveRoutine(targetPosition));
        AudioManager.inst.PlayCustomSound(fireballSpawn, 0.85f);
    }
    private IEnumerator MoveRoutine(Vector3 targetPosition)
    {
        float elapsed = 0f;
        float hitThresholdSqr = 0.1f * 0.1f; // 0.01f squared distance threshold

        while (elapsed < maxLifetime)
        {
            elapsed += Time.deltaTime;

            // Drive forward in 3D worldspace along local transform.forward
            transform.position += transform.forward * (projectileSpeed * Time.deltaTime);

            // Check squared distance to target position
            float sqrDistanceToTarget = (targetPosition - transform.position).sqrMagnitude;

            if (sqrDistanceToTarget <= hitThresholdSqr)
            {
                Debug.Log("Hit target!");

                if (pet != null)
                {
                    pet.DealDamage();
                    HitFxManager.inst.DragonFireHit(pet.target.position,transform.rotation);
                    AudioManager.inst.PlayCustomSound(fireballHit,0.85f);
                }


                gameObject.SetActive(false);
                moveRoutine = null;
                yield break;
            }

            yield return null;
        }

        // Lifetime expired without hit (Miss)
        gameObject.SetActive(false);
        moveRoutine = null;
    }
}
