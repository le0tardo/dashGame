using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] Animator anim;

    string aim = "aim";
    string launch = "launch";
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

        trail = GetComponentInChildren<TrailRenderer>();
    }

    public void IdleAnim()
    {
        anim.Play("spawn");
        aiming = false;
    }

    public void SetAim()
    {
        if (!aiming)
        {
            aiming = true;
            anim.SetTrigger(aim);
        }

    }

    public void LaunchAnim()
    {
        anim.SetTrigger(launch);
        aiming= false;
        aimPower = 0f;
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
        if(trail==null)trail=GetComponentInChildren<TrailRenderer>();

        if(trail!=null)trail.emitting = true;
        if (meleeSwingSound != null) AudioManager.inst.PlayCustomSound(meleeSwingSound, 0.25f);

    }
    public void DeactivateTrail()
    {
        if (trail != null) trail.emitting = false;
    }
}
