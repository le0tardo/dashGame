using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Melee,
        Moving,
        Falling,
        Dead
    }
    public PlayerState CurrentState { get; private set; } = PlayerState.Idle;

    [SerializeField] public PlayerMove move;
    [SerializeField] PlayerAnimations playerAnim;


    private void Start()
    {
        move=GetComponent<PlayerMove>();
        playerAnim=move.gameObject.GetComponentInChildren<PlayerAnimations>();
    }
    public void SetState(PlayerState newState)
    {
        CurrentState = newState;
    }

    public void TakeDamage(float dmg, Vector3 hitPos)
    {
        move.StartKnockBack(hitPos, dmg);
        SubtractHealth(dmg);
        CameraShake.inst.Shake(0.1f,dmg/4);
    }

    public void TakeLightDamage(float dmg, Vector3 hitPos)
    {
        move.StartKnockBack(hitPos, dmg/2);
        SubtractHealth(dmg);
        CameraShake.inst.Shake(0.05f, 0.5f);
    }

    public void Hurt(float dmg, Vector3 hitPos)
    {
        move.StartKnockBack(hitPos, 2.5f);
        CameraShake.inst.Shake(0.05f, 2f); //extra camera shake
        SubtractHealth(dmg);
    }

    void SubtractHealth(float dmg)
    {
        dmg = -dmg;
        LevelManager.inst.ChangeHealth(Mathf.RoundToInt(dmg)); //TODO: move to individual takeDamage for customsounds...
        AudioManager.inst.PlayHeroHurtSound();
        playerAnim.HurtAnim();
    }
    public void Heal(float heal)
    {
        AddHealth(heal);
    }
    void AddHealth(float heal)
    {
        LevelManager.inst.ChangeHealth(Mathf.RoundToInt(heal));
    }
}
