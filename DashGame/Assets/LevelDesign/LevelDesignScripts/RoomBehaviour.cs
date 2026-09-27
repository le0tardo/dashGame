using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class RoomBehaviour : MonoBehaviour
{
    [SerializeField] public List<GameObject> enemies = new List<GameObject>();
    [SerializeField] public List<GameObject> props = new List<GameObject>();
    [SerializeField] NavMeshSurface navMesh;

    private void Start()
    {
        if (RoomManager.inst.currentRoom != this.transform)
        {
            DeactivateRoom();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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
}
