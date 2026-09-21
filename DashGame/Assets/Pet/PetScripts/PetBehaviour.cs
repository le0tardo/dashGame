using UnityEngine;

public class PetBehaviour : MonoBehaviour
{
    Vector3 desiredPosition;
    [SerializeField] Transform playerTransform;
    [SerializeField] float distanceToPlayer;

    private void Start()
    {
        playerTransform=LevelManager.inst.playerMove.gameObject.transform;

    }

    private void Update()
    {
        desiredPosition = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
        
        distanceToPlayer=Vector3.Distance(desiredPosition, playerTransform.position);
    }
}
