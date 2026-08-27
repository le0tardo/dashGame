using UnityEngine;

public class BatAnimations : MonoBehaviour
{
    Animator anim;

    public bool moving=false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void MoveBat(bool move)
    {
        anim.SetBool("moving", move);
    }

    public void BatAttack()
    {
        anim.SetTrigger("attack");
    }

    public void BatHit()
    {
        anim.SetTrigger("hit");
    }
}
