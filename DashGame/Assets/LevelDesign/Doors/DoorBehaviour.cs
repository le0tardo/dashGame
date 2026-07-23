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
                Destroy(this.gameObject);
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

        //Destroy(this.gameObject);
    }
}
