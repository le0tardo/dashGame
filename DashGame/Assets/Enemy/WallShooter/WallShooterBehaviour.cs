using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WallShooterBehaviour : MonoBehaviour
{
    [SerializeField] LayerMask playerLayer;
    float rayDist = 10f;

    [SerializeField]bool playerInLOS;
    [SerializeField] WallShotBehaviour arrow;

    public bool SeesPlayer(Vector3 origin, Vector3 direction)
    {
        return Physics.Raycast(origin, direction, rayDist, playerLayer);
    }

    private void Update()
    {
        Vector3 origin= transform.position;
        Vector3 direction= transform.forward;

        playerInLOS = SeesPlayer(origin,direction);

        if (playerInLOS) { StartCoroutine(ShootRoutine()); } //Only start if its not already started?

    }
    void Shoot()
    {
        if (playerInLOS)
        {
            if (arrow != null && !arrow.fired)
            {
                arrow.FireArrow();
            }
        }
    }

    IEnumerator ShootRoutine()
    {
        //if !playerInLOS, exit coroutine
        Shoot();
        yield return new WaitForSeconds(1f);

    }
}
