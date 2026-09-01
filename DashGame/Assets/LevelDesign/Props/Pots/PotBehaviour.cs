using UnityEngine;

public class PotBehaviour : MonoBehaviour,IHittable
{
    enum PotType
    {
        One,
        Two,
        Thre
    }
    enum PotDrop
    {
        Stamina,
        Health,
        Gold
    }

    [SerializeField] PotType potType;
    [SerializeField] PotDrop dropType;
    [SerializeField] int dropAmount;
    [SerializeField] float shatterThreshold=10f;
    [SerializeField] float bounce = 0.33f;
    public float hitBounce => bounce;

    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip[] shatterSound;
    public void OnHit(Vector3 hitPosition, float power)
    {
        Vector3 fxPos = (transform.position + hitPosition) / 2;
        HitFxManager.inst.HitFX1(fxPos, power / 10);

        if (power <= shatterThreshold)
        {
            //weak tap
            CameraShake.inst.Shake(0.1f, 0.5f);
            AudioManager.inst.PlayCustomSound(hitSound,power/100);
        }
        else
        {
            //EnemyShatterManager
            CameraShake.inst.Shake(0.1f, 1f);
            int r=Random.Range(0,shatterSound.Length);
            AudioManager.inst.PlayCustomSound(shatterSound[r], power / 100);
            Die();

        }
    }
    void Die()
    {
        EnemyShatterManager.inst.ShatterPot(transform.position);

        switch (dropType)
        {
            case PotDrop.Stamina:
                OrbPool.inst.SpawnOrbs(dropAmount, transform.position);
                break;
            case PotDrop.Health:
                OrbPool.inst.SpawnOrbs(dropAmount, transform.position);
                break;
            case PotDrop.Gold:
                OrbPool.inst.SpawnOrbs(dropAmount, transform.position);
                break;
        }

        this.gameObject.SetActive(false);
    }
}
