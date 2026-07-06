using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    [SerializeField] List<GameObject> enemies = new List<GameObject>();

    private void Start()
    {
        PopulateEnemies(); //do this manually too, but keep if i forget someone...
        if (RoomManager.inst.currentRoom != this.transform)
        {
            DeactivateRoom();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("player entered:" + name);
            RoomManager.inst.currentRoom = this.transform;
            RoomManager.inst.ChangeRoom(this.transform);

            ActivateRoom();
        }

        if (other.CompareTag("Enemy"))
        {
            if (!enemies.Contains(other.gameObject))
            {
                enemies.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("player exited:" + name);
            DeactivateRoom();
        }
    }

    void PopulateEnemies()
    {
        //enemies.Clear();
        EnemyMove[] childEnemies = GetComponentsInChildren<EnemyMove>(true);

        foreach (EnemyMove enemy in childEnemies)
        {
            if (!enemies.Contains(enemy.gameObject))
            {
                enemies.Add(enemy.gameObject);
            }
        }

    }
    void ActivateRoom()
    {
        foreach (GameObject enemy in enemies) 
        {
            if (enemy != null)
            {
                enemy.SetActive(true);
            }
        }

    }
    void DeactivateRoom()
    {
        print(name + " has been deactivated");

        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.SetActive(false);
            }
        }
    }
}
