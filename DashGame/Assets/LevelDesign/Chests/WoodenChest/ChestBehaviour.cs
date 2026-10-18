using UnityEngine;

public class ChestBehaviour : MonoBehaviour, IHittable
{
    [SerializeField] float bounce = 0.5f;
    public float hitBounce => bounce;

    [SerializeField] Animator chestAnimator;
    [SerializeField] AudioClip chestOpenSound;
    [SerializeField] AudioClip chestImpactSound;
    [SerializeField] AudioClip chestBreakSound;
    [SerializeField] ParticleSystem openParticle;
    [SerializeField] int goldAmount;
    [SerializeField] bool opened;
    public void OnHit(Vector3 hitPosition, float power)
    {
        Vector3 fxPos = (transform.position + hitPosition) / 2;

        if (!opened)
        {
            OpenChest();
            AudioManager.inst.PlayCustomSound(chestOpenSound, 0.5f);
            opened = true;
        }
        else
        {
            ShatterManager.inst.ShatterCrate(fxPos);
            AudioManager.inst.PlayCustomSound(chestBreakSound,0.5f);
            this.gameObject.SetActive(false);
        }



        HitFxManager.inst.HitFX1(fxPos, power / 10);
        CameraShake.inst.Shake(0.1f, 1f);
        AudioManager.inst.PlayCustomSound(chestImpactSound,0.5f);
    }

    void OpenChest()
    {
        if (openParticle != null) openParticle.Play();
        chestAnimator.SetTrigger("open");
        Vector3 goldSpawnPosition = transform.position + transform.forward * 4f;
        OrbPool.inst.SpawnGoldCoins(goldAmount,goldSpawnPosition);
    }
}
