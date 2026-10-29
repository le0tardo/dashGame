using UnityEngine;
using System.Collections;
public class PlayerMelee : MonoBehaviour
{
    [SerializeField] public float meleeDamage;
    [SerializeField] public float meleeDuration;
    CapsuleCollider meleeCollider;
    [SerializeField] Animator anim;
    int meleeChain = 1;
    bool canMelee = true;
    float meleeCoolDown = 0.25f;
    private void Awake()
    {
        meleeCollider = GetComponent<CapsuleCollider>();
        meleeCollider.enabled = false;
    }

    public void MeleeAnimation()
    {
        if (canMelee)
        {
            anim.SetInteger("meleeChain", meleeChain);
            anim.SetTrigger("melee");

            if (meleeChain < 3)
            {
                meleeChain++;
            }
            else
            {
                meleeChain = 1;
            }

            canMelee = false;
            CancelInvoke(nameof(ToggleMeleeBool));
            Invoke(nameof(ToggleMeleeBool), meleeCoolDown);

            Invoke(nameof(MeleeHit), 0.15f);
        }
    }

    void ToggleMeleeBool()
    {
        if (!canMelee) canMelee = true;
    }

    public void MeleeHit()
    {
        meleeDamage = LevelManager.inst.weaponDamage;
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
            if (other.TryGetComponent<IEnemy>(out IEnemy enemyInterface))
            {
                enemyInterface.TakeDamage(LevelManager.inst.weaponDamage);
                AudioManager.inst.PlayMeleeSound();
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
