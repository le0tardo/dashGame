using UnityEngine;

public class GhostGrave : MonoBehaviour, IHittable
{
    [SerializeField] float bounce = 1.1f;
    [SerializeField] float health=10;
    public float hitBounce => bounce;

    public void OnHit(Vector3 hitPos, float power)
    {
        float force=Mathf.Floor(power/10);
        health-=force;

        if (health <= 0)
        {
            Kill();
        }

        //default feedback
        Vector3 fxPos = (hitPos + transform.position) / 2;
        HitFxManager.inst.HitFX1(fxPos, force);
        CameraShake.inst.Shake(0.1f, 1f);
        AudioManager.inst.PlayImpactSound(force);
    }
     void Kill()
    {
        Destroy(this.gameObject);
    }
}
