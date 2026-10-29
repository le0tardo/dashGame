using UnityEngine;

public class MayhemRoomBehaviour : MonoBehaviour
{
    [SerializeField] bool mayhem=false;
    [SerializeField] bool cleared=false;
    [SerializeField] MayhemDoorBehaviour[] doors;
    [SerializeField] AudioClip doorOpenSound;
    [SerializeField] AudioClip doorCloseSound;
    [SerializeField] RoomBehaviour room;
    [SerializeField] int remainingEnemies;

    [SerializeField] Transform[] safeSpaces;

    private void Awake()
    {
        room = GetComponent<RoomBehaviour>();
        remainingEnemies=room.enemies.Count;
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player") && other is CapsuleCollider cap)
        {
            if (!mayhem && !cleared)
            {
                mayhem = true;
                StartMayhem();
            }
        }
    }

    private void Update()
    {
        if (mayhem)
        {
            remainingEnemies = room.enemies.Count;

            if (remainingEnemies <= 0)
            {
                StopMayhem();
                mayhem = false;
                cleared = true;
            }
        }
    }
    void StartMayhem()
    {
        foreach (var door in doors)
        {
            door.CloseDoor();
            AudioManager.inst.PlayCustomSound(doorCloseSound, 1f);
        }

    }

    void StopMayhem()
    {
        foreach(var door in doors)
        {
           if(door!=null) door.OpenDoor();
            AudioManager.inst.PlayCustomSound(doorOpenSound, 1f);
        }
    }
}
