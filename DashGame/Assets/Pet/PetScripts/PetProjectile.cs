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

        while (transform.position != targetPosition && elapsed < maxLifetime)
        {
            elapsed += Time.deltaTime;

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                projectileSpeed * Time.deltaTime
            );

            yield return null;
        }


        if (pet != null)
        {
            pet.DealDamage();

            if (pet.target != null)
            {
                HitFxManager.inst.DragonFireHit(pet.target.position, transform.rotation);
            }
            else
            {

                HitFxManager.inst.DragonFireHit(targetPosition, transform.rotation);
            }

            AudioManager.inst.PlayCustomSound(fireballHit, 0.85f);
        }

        gameObject.SetActive(false);
        moveRoutine = null;
    }
}
