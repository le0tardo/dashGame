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

    [SerializeField] float health;
    float maxHealth;
    public float mass;
    public PlayerState CurrentState { get; private set; } = PlayerState.Idle;

    PlayerMove move;
    private void Start()
    {
        maxHealth = health;
        move=GetComponent<PlayerMove>();
    }
    public void SetState(PlayerState newState)
    {
        CurrentState = newState;
    }

    public void TakeDamage(float dmg, Vector3 hitPos)
    {
        health-=dmg;
        Mathf.Clamp(health, 0, maxHealth);
        if (health <= 0) {Destroy(this.gameObject);}

        //move.KnockBack(hitPos,dmg); //regular knockback with the same movement system
        move.StartKnockBack(hitPos, dmg);
    }


}
