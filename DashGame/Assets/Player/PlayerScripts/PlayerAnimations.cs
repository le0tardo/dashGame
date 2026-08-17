using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] Animator anim;

    string idle = "idle";
    string aim = "aim";
    string launch = "launch";
    string stop = "stop";

    [SerializeField] bool aiming=false;
    [SerializeField] PlayerMove moveScript;
    [SerializeField] float playbackSpeed = 1f;
  
    private void Awake()
    {
       if(anim==null) anim = GetComponentInChildren<Animator>();
        print("anim script exists");
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
    }

    public void IdleAnim()
    {
        print("playing idle animation");
        anim.SetTrigger(idle);
    }

    public void AimAnim()
    {
        print("playing aim animation");
        anim.SetTrigger(aim);
        aiming = true;
    }

    public void LaunchAnim()
    {
        print("playing launch animation");
        anim.SetTrigger(launch);
        aiming= false;
    }
    public void StopAnim()
    {
        anim.SetTrigger(stop);
    }
}
