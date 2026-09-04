using UnityEngine;

public class PropBehaviour : MonoBehaviour,IHittable
{
    enum PropType
    {
        Pot,
        Crate,
        Barrel
    }
    enum PropDrop
    {
        Stamina,
        Health,
        Gold
    }

    [SerializeField] PropType propType;
    [SerializeField] PropDrop dropType;
    [SerializeField] int dropAmount;
    [SerializeField] float shatterThreshold=10f;
    [SerializeField] float bounce = 0.33f;
    public float hitBounce => bounce;

    [SerializeField] AudioClip[] hitSounds;
    [SerializeField] AudioClip[] shatterSounds;
    public void OnHit(Vector3 hitPosition, float power)
    {
        Vector3 fxPos = (transform.position + hitPosition) / 2;
        HitFxManager.inst.HitFX1(fxPos, power / 10);

        if (power <= shatterThreshold)
        {
            //weak tap
            CameraShake.inst.Shake(0.1f, 0.5f);
            int r = Random.Range(0, hitSounds.Length);
            AudioManager.inst.PlayCustomSound(hitSounds[r],power/100);
        }
        else
        {
            //ShatterManager
            CameraShake.inst.Shake(0.1f, 1f);
            int r=Random.Range(0,shatterSounds.Length);
            AudioManager.inst.PlayCustomSound(shatterSounds[r], power / 100);
            Die();

        }
    }
    void Die()
    {
        switch (propType)
        {
            case PropType.Pot:
                ShatterManager.inst.ShatterPot(transform.position);
                break;
            case PropType.Crate:
                ShatterManager.inst.ShatterCrate(transform.position);
                break;
            case PropType.Barrel:
                ShatterManager.inst.ShatterBarrel(transform.position);
                break;
        }



        switch (dropType)
        {
            case PropDrop.Stamina:
                OrbPool.inst.SpawnOrbs(dropAmount, transform.position);
                break;
            case PropDrop.Health:
                OrbPool.inst.SpawnOrbs(dropAmount, transform.position);
                break;
            case PropDrop.Gold:
                OrbPool.inst.SpawnOrbs(dropAmount, transform.position);
                break;
        }

        this.gameObject.SetActive(false);
    }
}
