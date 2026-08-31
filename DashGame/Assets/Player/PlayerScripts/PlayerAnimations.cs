using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] Animator anim;

    string idle = "idle";
    string aim = "aim";
    string launch = "launch";
    string dashHit = "hit";
    string stop = "stop";

    [SerializeField] bool aiming=false;
    [SerializeField] PlayerMove moveScript;
    [SerializeField] float playbackSpeed = 1f;
    [SerializeField] public float aimPower=0f;

    [SerializeField]FlashRed flashScript;
  
    private void Awake()
    {
        if(anim==null) anim = GetComponentInChildren<Animator>();

        moveScript = GetComponentInParent<PlayerMove>();
        if(flashScript==null)flashScript = GetComponent<FlashRed>();
    }

    private void Update()
    {
        if (moveScript.moveState == PlayerMove.MoveState.Dashing)
        {
            playbackSpeed=1f+(moveScript.currentVelocity.magnitude/100);
        }
        else
        {
            playbackSpeed = 1f;
        }
        anim.speed = playbackSpeed;

        if (aiming)
        {
            anim.Play("aim", 0, aimPower);
        }

        //debug
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                HurtAnim();
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                BounceAnim();
            }
        }
    }

    public void IdleAnim()
    {
        anim.SetTrigger(idle);
    }

    public void AimAnim()
    {
        //anim.SetTrigger(aim); //played in update instead.
        aiming = true;
    }

    public void LaunchAnim()
    {
        anim.SetTrigger(launch);
        aiming= false;
        aimPower = 0f;
    }

    public void DashHitAnim()
    {
        anim.SetTrigger(dashHit);
    }

    public void StopAnim()
    {
        anim.SetTrigger(stop);
    }

    public void HurtAnim()
    {
        anim.SetTrigger("hurt");

        if (flashScript != null)
        {
            flashScript.Flash();
            print("flash called");
        }
        else
        {
            print("null??");
        }
    }
    public void BounceAnim()
    {
        anim.SetTrigger("bounce");
    }

}
