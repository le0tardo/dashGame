using UnityEngine;

public class WallBumperBehaviour : MonoBehaviour,IHittable
{
    [SerializeField] float bounce = 1.5f;
    public float hitBounce => bounce;
    [SerializeField] AudioClip[] boing;

    public void OnHit(Vector3 hitPosition, float power)
    {


        Vector3 fxPos = (transform.position + hitPosition) / 2;
        HitFxManager.inst.HitFX1(fxPos, power / 10);
        CameraShake.inst.Shake(0.1f, 2f);

        if (boing != null)
        {
            int r = Random.Range(0, boing.Length);
            float vol = power / 100;
            vol = vol * 2;
            Mathf.Clamp(vol, 0.2f, 1f);
            AudioManager.inst.PlayCustomSound(boing[r], vol);
        }
        else
        {
            AudioManager.inst.PlayImpactSound(power);
        }

    }
}
