using UnityEngine;

public class RoomChecker : MonoBehaviour
{
    [SerializeField] PlayerMove player;
    [SerializeField] Transform globalRoom;
    [SerializeField] Transform playerRoom;
    [SerializeField] BoxCollider roomCheckCollider;

    private void Start()
    {
        InvokeRepeating(nameof(CheckPlayerRoom),1,1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Room"))
        {
            playerRoom = other.transform;
        }
    }

    void CheckPlayerRoom()
    {
        globalRoom = RoomManager.inst.currentRoom;
        transform.position=player.transform.position;

        if (playerRoom != globalRoom)
        {
            print("Player room mismatch!");
        }

    }
}
