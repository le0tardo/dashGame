using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    [SerializeField] float health=100;
    float maxHealth;
    [SerializeField] public bool breakable=true;
    private void Start()
    {
        maxHealth = health;
    }

    public void KnockOnDoor(float dmg)
    {
        if (breakable)
        {
            health -= dmg;
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
        Destroy(this.gameObject);
    }
}
