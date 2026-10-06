using Unity.VisualScripting;
using UnityEngine;

public class WallShooterBehaviour : MonoBehaviour
{
    [SerializeField] LayerMask playerLayer;
    float rayDist = 10f;

    private readonly RaycastHit[] hitResults = new RaycastHit[1];
    [SerializeField]bool playerInLOS;
    [SerializeField] float coolDown=1f;
    [SerializeField] WallShotBehaviour arrow;

    private void OnEnable()
    {
        InvokeRepeating(nameof(Shoot), 1f, coolDown);
    }
    public bool SeesPlayer(Vector3 origin, Vector3 direction)
    {
        return Physics.Raycast(origin, direction, rayDist, playerLayer);
    }

    private void Update()
    {
        Vector3 origin= transform.position;
        Vector3 direction= transform.forward;

        playerInLOS = SeesPlayer(origin,direction);

    }

    void Shoot()
    {
        if (playerInLOS)
        {
            if (arrow != null)
            {
                arrow.FireArrow();
            }
        }
    }
}
