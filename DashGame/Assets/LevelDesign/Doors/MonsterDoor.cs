using System.Collections.Generic;
using UnityEngine;

public class MonsterDoor : MonoBehaviour
{
    [SerializeField] List<GameObject> enemies = new List<GameObject>();
    [SerializeField] bool locked = true;

    [SerializeField] AudioClip slide;

    [SerializeField] Animator anim;

    [SerializeField] GameObject dust;

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
        anim.SetTrigger("open");
        Invoke("Dust", 1f);
        AudioManager.inst.PlayCustomSound(slide, 1f);
        BoxCollider bx=GetComponentInChildren<BoxCollider>();
        bx.enabled = false;
    }
     void Dust()
    {
        dust.SetActive(true);
    }
}
