using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    [SerializeField] float health=100;
    float maxHealth;
    [SerializeField] public bool breakable=true;

    [SerializeField] Animator anim;
    private void Start()
    {
        maxHealth = health;
    }

    public void KnockOnDoor(float dmg)
    {
        if (breakable)
        {
            health -= dmg;
            if (anim != null) { anim.SetTrigger("hit"); }

            GameObject pl = GameObject.Find("Player");
            Vector3 midpoint = (transform.position + pl.transform.position) / 2f;

            HitFxManager.inst.HitFX1(midpoint, dmg);
            if (health <= 0)
            {
                BreakDoor();
            }
        }
        else
        {
            if (LevelManager.inst.keys > 0)
            {
                LevelManager.inst.keys--;
                OpenKeyDoor();

            }
            else
            {
                //wobble anim
                if (anim != null)
                {
                    anim.SetTrigger("hit");
                }
            }
        }
    }

    void BreakDoor()
    {
        DoorFracture df=GetComponentInChildren<DoorFracture>();
        if (df == null) print("no door fracture script");
        else
        {
            df.Shatter();
        }
        BoxCollider bx=GetComponent<BoxCollider>();
        bx.enabled = false;
    }

    void OpenKeyDoor()
    {
        if (anim != null) anim.SetTrigger("open");
        Invoke("DestroyKeyDoor", 0.5f);
    }

    void DestroyKeyDoor()
    {
        Destroy(this.gameObject);
    }
}
