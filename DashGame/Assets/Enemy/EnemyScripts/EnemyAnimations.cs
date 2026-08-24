using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] EnemyMove move;
    [SerializeField] EnemyCombat combat;

    public bool isWalking=false;
    public bool isAttacking=false;
    public bool poolBall=false;

    private void Start()
    {
        if(anim==null) anim = GetComponent<Animator>();
        if(move==null) move=GetComponentInParent<EnemyMove>();
        if(combat==null) combat=GetComponentInParent<EnemyCombat>();
    }

    private void Update()
    {
        if (move.agent.enabled) //zombie mode
        {
            if (poolBall) { poolBall = false; }
            //combat check
            if (combat.inCombat)
            {
                if (!isAttacking)
                {
                    anim.SetTrigger("combat");
                    isAttacking = true;
                    isWalking = false;
                }
            }
            else//not in combat range
            {
                if (move.agent.velocity.sqrMagnitude > 0.01f) //agent is moving
                {
                    if (!isWalking)
                    {
                        isWalking = true;
                        anim.SetTrigger("walk");
                    }
                }
                else //agent is still
                {
                    if (isWalking)
                    {
                        isWalking = false;
                        anim.SetTrigger("idle");
                    }
                }
                isAttacking=false;
            }
        }
        else //pool ball mode
        {
            isWalking=false;
            isAttacking = false;
            if (!poolBall)
            {
                anim.SetTrigger("hurt");
                poolBall = true;
            }

        }
    }

}
