using UnityEngine;
using System.Collections;

public class PetProjectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed=10f;
    [SerializeField] private float maxLifetime = 3f;

    [SerializeField] Vector3 targetPosition = Vector3.zero;

    private Coroutine moveRoutine=null;

    public void FireProjectile(Vector3 _targetPosition)
    {
        targetPosition= new Vector3 (_targetPosition.x,transform.position.y,_targetPosition.z);

        Vector3 direction = (targetPosition - transform.position).normalized;


        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        if (moveRoutine != null) StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(MoveRoutine(targetPosition));
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

                // Insert damage / visual hit impact logic here
                //do i want the projectile to be able to miss?
                //or is it more just a DOT to a random enemy in the room?
                //DOT i think. spawn a collider that looks for IEnemy?

                // Disable projectile and exit coroutine immediately
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
