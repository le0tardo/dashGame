using UnityEngine;

public class BatAnimations : MonoBehaviour
{
    Animator anim;
    FlashRed flash;
    public bool moving=false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        flash = GetComponent<FlashRed>();
        float r=Random.value;
        anim.Play(0, 0, r);

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
        if(flash!=null)flash.Flash();
    }
}
