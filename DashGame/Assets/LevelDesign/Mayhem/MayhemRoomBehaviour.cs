using UnityEngine;

public class MayhemRoomBehaviour : MonoBehaviour
{
    [SerializeField] bool mayhem=false;
    [SerializeField] bool cleared=false;
    [SerializeField] MayhemDoorBehaviour[] doors;
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
                EnterPlayer();
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
        }

        //start mayhem spawners
    }

    void StopMayhem()
    {
        foreach(var door in doors)
        {
            door.OpenDoor();
        }
    }

    void EnterPlayer()
    {
        var player=LevelManager.inst.playerMove;

        Vector3 closestPosition = GetClosestSafeSpace(player.transform.position);

        print("player velocity magnitude on enter: "+player.currentVelocity.magnitude);

        if (player.currentVelocity.magnitude < 10)
        {
            player.WarpPlayer(closestPosition);
        }
    }
    Vector3 GetClosestSafeSpace(Vector3 entryPoint)
    {
        if (safeSpaces == null || safeSpaces.Length == 0)
        {
            Debug.LogWarning("Safe spaces array is empty or null!");
            return entryPoint;
        }

        Transform closestTransform = safeSpaces[0];
        float minSqDistance = (safeSpaces[0].position - entryPoint).sqrMagnitude;

        for (int i = 1; i < safeSpaces.Length; i++)
        {
            if (safeSpaces[i] == null) continue;

            // sqrMagnitude avoids the expensive square root calculation of Vector3.Distance
            float sqDistance = (safeSpaces[i].position - entryPoint).sqrMagnitude;
            if (sqDistance < minSqDistance)
            {
                minSqDistance = sqDistance;
                closestTransform = safeSpaces[i];
            }
        }

        return closestTransform.position;
    }
}
