using UnityEngine;

public class LobbyHeroAnimator : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] AudioClip clip;
    [SerializeField] GameObject fadeToBlack;
    public void EnterDungeonAnimation()
    {
        anim.SetTrigger("enter");
        AudioManager.inst.PlayCustomSound(clip, 1f);
        if(fadeToBlack!=null)fadeToBlack.SetActive(true);
    }

    public void LevelUpAnimation()
    {
        anim.SetTrigger("lvlUp");
        AudioManager.inst.PlayCustomSound(clip, 1f);
    }
    public void ActivateTrail()
    {
        //this just shuts the error off lol
    }
    public void DeactivateTrail()
    {
        //this just shuts the error off lol
    }
}
