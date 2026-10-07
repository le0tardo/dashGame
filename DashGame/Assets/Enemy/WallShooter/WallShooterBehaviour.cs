using UnityEngine;

public class WallShooterBehaviour : MonoBehaviour
{
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float rayDist = 10f;
    [SerializeField] private float maxCoolDown = 1f;
    float coolDown = 0f;

    [SerializeField]bool playerInLOS;
    [SerializeField] WallShotBehaviour arrow;

    float nectFireTime;
    public bool SeesPlayer(Vector3 origin, Vector3 direction)
    {
        return Physics.Raycast(origin, direction, rayDist, playerLayer);
    }

    private void Update()
    {
        Vector3 origin= transform.position;
        Vector3 direction= transform.forward;

        playerInLOS = SeesPlayer(origin,direction);

        if (playerInLOS)
        {
            if (coolDown <= 0)
            {
                Shoot();
            }
            else
            {
                coolDown-=Time.deltaTime;
            }
        }
        else
        {
            coolDown = 0;
        }

    }
    void Shoot()
    {
        if (playerInLOS)
        {
            if (arrow != null && !arrow.fired)
            {
                arrow.FireArrow();
                coolDown = maxCoolDown;
            }
        }
    }

}
