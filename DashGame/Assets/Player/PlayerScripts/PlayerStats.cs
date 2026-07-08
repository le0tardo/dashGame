using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Moving,
        Falling,
        Dead
    }
    public PlayerState CurrentState { get; private set; } = PlayerState.Idle;

    PlayerMove move;
    private void Start()
    {
        move=GetComponent<PlayerMove>();
    }
    public void SetState(PlayerState newState)
    {
        CurrentState = newState;
    }

    public void TakeDamage(float dmg, Vector3 hitPos)
    {
        move.StartKnockBack(hitPos, dmg);
        SubtractHealth(dmg);
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
        LevelManager.inst.ChangeHealth(Mathf.RoundToInt(dmg));
    }
}
