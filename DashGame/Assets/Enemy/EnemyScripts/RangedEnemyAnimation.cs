using UnityEngine;

public class RangedEnemyAnimation : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] EnemyRangedBehavour ranged;
    [SerializeField] bool attacking=false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        ranged=GetComponentInParent<EnemyRangedBehavour>();
    }

    private void Update()
    {
        if (ranged.aimed && !attacking)
        {
            anim.SetTrigger("aimed");
            attacking=true;
        }

        if (ranged.hurt)
        {
            anim.SetTrigger("hurt");
            ranged.hurt = false;
        }
    }

    public void AnimationTriggerShoot()
    {
        if (ranged!=null)
        {
            ranged.Shoot();
        }
    }
}
