using UnityEngine;

public class SpikeWallBehaviour : MonoBehaviour,IHittable
{
    [SerializeField] float damage=1f;
    [SerializeField] float bounce = 0.5f;
    [SerializeField] AudioClip shankSound;
    public float hitBounce => bounce;

    public void OnHit(Vector3 hitPosition, float power)
    {
        print("player hit spike wall");

        Vector3 fxPos = (transform.position + hitPosition) / 2;
        HitFxManager.inst.HitFX1(fxPos, power / 10);
        CameraShake.inst.Shake(0.1f, 1f);

        float vol = power / 100;
        vol = vol * 2;
        Mathf.Clamp(vol, 0.2f, 1f);

        if (shankSound != null) AudioManager.inst.PlayCustomSound(shankSound,vol);

        LevelManager.inst.playerStats.SubtractHealth(damage);
    }
}
