using UnityEngine;

public class GhostBehaviour : MonoBehaviour
{
    private enum ghostState {Turning, Charging }
    [SerializeField] ghostState state;
    [SerializeField] Transform target;
    [SerializeField] Transform roomCentre;
    string roomTag = "Room";

    [Header("move stats")]
    [SerializeField] float turnSpeed=1f;
    [SerializeField] float chargeSpeed = 1f;
    [SerializeField] float maxChargeSpeed = 25f;
    [SerializeField] bool accelerate=false;
    [SerializeField] float acceleration;
    [SerializeField] LayerMask hitLayer;
    private float targetAngleThreshold = 5f;
    private Vector3 chargeDirection;

    [Header("Attack stats")]
    [SerializeField] bool canHit = true;
    [SerializeField] float damage = 1f;

    private void Start()
    {
        target=LevelManager.inst.playerMove.gameObject.transform;
        roomCentre=RoomManager.inst.currentRoom.transform;
        state = ghostState.Turning;
    }

    private void Update()
    {
        if(target==null)return;

        switch (state)
        {
            case ghostState.Turning:
                Turn();
                break;

            case ghostState.Charging:
                Charge();
                break;
        }
    }
    void Turn()
    {
        bool isOverlapping = Physics.CheckSphere(transform.position, 1f, hitLayer);

        if (isOverlapping)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                roomCentre.position,
                5f * Time.deltaTime
            );
        }

        Vector3 targetDirection = (target.position - transform.position);
        targetDirection.y = 0f;

        if (targetDirection == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

        float angleToTarget = Quaternion.Angle(transform.rotation, targetRotation);

        if (angleToTarget <= targetAngleThreshold)
        {
            if (accelerate)
            {
                chargeSpeed = 1;
            }

            Vector3 exactTargetDir = (target.position - transform.position);
            exactTargetDir.y = 0f;
            chargeDirection = exactTargetDir.normalized;

            if (chargeDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(chargeDirection);
            }

            canHit = true;
            AudioManager.inst.PlaySwoosh(0.5f);
            state = ghostState.Charging;
        }
    }
    void Charge()
    {
        if (accelerate && chargeSpeed < maxChargeSpeed)
        {
            chargeSpeed += (acceleration * Time.deltaTime);
        }
        transform.position += chargeDirection * (chargeSpeed * Time.deltaTime);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(roomTag))
        {
            state = ghostState.Turning;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&canHit)
        {
            print("ghost on player");
            PlayerStats player=other.GetComponent<PlayerStats>();
            player.TakeDamage(damage,transform.position);
            canHit = false;
        }
    }
}
