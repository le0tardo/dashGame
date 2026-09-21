using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    [SerializeField] public float meleeDamage;
    [SerializeField] public float meleeDuration;
    CapsuleCollider meleeCollider;

    private void Awake()
    {
        meleeCollider = GetComponent<CapsuleCollider>();
        meleeCollider.enabled = false;
    }

    public void MeleeHit()
    {
        if(!meleeCollider.enabled) meleeCollider.enabled=true;
        Invoke("EndMeleeHit",meleeDuration);
    }

    void EndMeleeHit()
    {
        if(meleeCollider.enabled)meleeCollider.enabled=false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyCombat enemy=other.GetComponent<EnemyCombat>();
            if (enemy != null)
            {
                enemy.TakeDamage(meleeDamage);
                print(other.name + " took " + meleeDamage + " melee damage");
            }
        }

        if (other.CompareTag("BreakableProp"))
        {
            PropBehaviour prop=other.GetComponent<PropBehaviour>();
            if (prop != null)
            {
                prop.MeeleBreak();
            }
        }
    }

}
