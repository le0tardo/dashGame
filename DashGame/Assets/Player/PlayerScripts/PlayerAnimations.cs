using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] Animator anim;

    string idle = "idle";
    string aim = "aim";
    string launch = "launch";
    string dashHit = "hit";
    string stop = "stop";

    [SerializeField] public bool aiming=false;
    [SerializeField] PlayerMove moveScript;
    [SerializeField] float playbackSpeed = 1f;
    [SerializeField] public float aimPower=0f;

    [SerializeField] TrailRenderer trail;
    [SerializeField] AudioClip meleeSwingSound;

    [SerializeField]FlashRed flashScript;
  
    private void Awake()
    {
        if(anim==null) anim = GetComponentInChildren<Animator>();

        moveScript = GetComponentInParent<PlayerMove>();
        if(flashScript==null)flashScript = GetComponent<FlashRed>();
    }

    public void IdleAnim()
    {
        anim.SetTrigger(idle);
        aiming = false;
    }

    public void AimAnim()
    {
        print("does this ever happen?");
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
        }
    }
    public void BounceAnim()
    {
        anim.SetTrigger("bounce");
    }

    public void FallAnim()
    {
        anim.SetTrigger("fall");
    }

    public void DeathAnim()
    {
        anim.SetTrigger("die");
    }

    public void ActivateTrail()
    {
        if(trail!=null)trail.emitting = true;
        if (meleeSwingSound != null) AudioManager.inst.PlayCustomSound(meleeSwingSound, 0.25f);

    }
    public void DeactivateTrail()
    {
        if (trail != null) trail.emitting = false;
    }
}
