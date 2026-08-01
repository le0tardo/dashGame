using UnityEngine;

public class WallBehaviour : MonoBehaviour, IHittable
{
    [SerializeField] float bounce=0.5f;//default
    public float hitBounce =>bounce;

    [SerializeField] string customString;
    public void OnHit(Vector3 hitPosition, float power)
    {
        Vector3 fxPos = (transform.position + hitPosition) / 2;
        HitFxManager.inst.HitFX1(fxPos, power / 10);
        CameraShake.inst.Shake(0.1f,1f);

        AudioManager.inst.PlayImpactSound(power);
    }
}
