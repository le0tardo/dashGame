using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] Animator anim;

    string idle = "idle";
    string aim = "aim";
    string launch = "launch";
    string stop = "stop";

    private void Awake()
    {
       if(anim==null) anim = GetComponentInChildren<Animator>();
        print("anim script exists");
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
    }

    public void LaunchAnim()
    {
        print("playing launch animation");
        anim.SetTrigger(launch);
    }
    public void StopAnim()
    {
        anim.SetTrigger(stop);
    }
}
