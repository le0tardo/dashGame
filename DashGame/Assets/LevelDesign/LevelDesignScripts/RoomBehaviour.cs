using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class RoomBehaviour : MonoBehaviour
{
    [SerializeField] public List<GameObject> enemies = new List<GameObject>();
    [SerializeField] public List<GameObject> props = new List<GameObject>();
    [SerializeField] NavMeshSurface navMesh;
    [SerializeField] Transform[] entryPoints;

    private void Start()
    {
        if (RoomManager.inst.currentRoom != this.transform)
        {
            DeactivateRoom();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other is CapsuleCollider cap)
        {
            if (RoomManager.inst.currentRoom == this.transform)
            {
                return;
            }
            else
            {
                RoomManager.inst.currentRoom = this.transform;
                RoomManager.inst.ChangeRoom(this.transform);

                ActivateRoom();

               if(RoomManager.inst.visitedRooms.Count>1) EnterPlayer(); //prevent this on first room spawn...
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DeactivateRoom();
        }
    }

    void ActivateRoom()
    {
        if (!RoomManager.inst.visitedRooms.Contains(this.transform))
        {
            RoomManager.inst.AddRoomToList(this.transform);
        }

        int enemiesActivated = 0;
        if (navMesh != null)
        {
            navMesh.enabled = true;
        }

        foreach (GameObject enemy in enemies) 
        {
            if (enemy != null)
            {
                enemy.SetActive(true);
                enemiesActivated++;
            }
        }
        foreach (GameObject prop in props)
        {
            if (prop != null)
            {
                prop.SetActive(true);
            }
        }
    }
    void DeactivateRoom()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.SetActive(false);
            }
        }
        foreach (GameObject prop in props)
        {
            if (prop != null)
            {
                prop.SetActive(false);
            }
        }

        if (navMesh != null)
        {
            navMesh.enabled = false;
        }
    }

    void EnterPlayer()
    {
        var player = LevelManager.inst.playerMove;

        Vector3 closestPosition = GetClosestSafeSpace(player.transform.position);

        if (player.currentVelocity.magnitude < 10)
        {
            Vector3 warpTo = new Vector3(closestPosition.x, player.transform.position.y, closestPosition.z);
            player.WarpPlayer(warpTo);
        }
    }
    Vector3 GetClosestSafeSpace(Vector3 entryPoint)
    {
        if (entryPoints == null || entryPoints.Length == 0)
        {
            Debug.LogWarning("Safe spaces array is empty or null!");
            return entryPoint;
        }

        Transform closestTransform = entryPoints[0];
        float minSqDistance = (entryPoints[0].position - entryPoint).sqrMagnitude;

        for (int i = 1; i < entryPoints.Length; i++)
        {
            if (entryPoints[i] == null) continue;

            // sqrMagnitude avoids the expensive square root calculation of Vector3.Distance
            float sqDistance = (entryPoints[i].position - entryPoint).sqrMagnitude;
            if (sqDistance < minSqDistance)
            {
                minSqDistance = sqDistance;
                closestTransform = entryPoints[i];
            }
        }

        return closestTransform.position;
    }
}
