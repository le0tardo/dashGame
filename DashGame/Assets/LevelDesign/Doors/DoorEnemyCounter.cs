using System.Collections.Generic;
using UnityEngine;

public class DoorEnemyCounter : MonoBehaviour
{
    [SerializeField] List<GameObject> enemies = new List<GameObject>();
    [SerializeField] bool locked = true;

    private void Update()
    {
        if (locked)
        {
            enemies.RemoveAll(enemy => enemy == null);

            if (enemies.Count <= 0)
            {
                Unlock();
            }
        }
    }
    void Unlock()
    {
        locked = false;
        //TODO ofc a nice courutine or smth here
        //or just an animation and kill the collider?
        //for now:
        BoxCollider bx=GetComponentInChildren<BoxCollider>();
        bx.enabled = false;
        MeshRenderer rend=GetComponentInChildren<MeshRenderer>();
        rend.enabled = false;
    }
}
