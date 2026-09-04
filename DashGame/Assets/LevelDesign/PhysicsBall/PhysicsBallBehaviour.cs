using UnityEngine;

public class PhysicsBallBehaviour : MonoBehaviour,IHittable
{
    [SerializeField] float bounce = 0.85f;
    public float hitBounce => bounce;

    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void OnHit(Vector3 hitPosition, float power)
    {
        Vector3 powerVector = new Vector3(power,power,power);
        rb.AddForceAtPosition(powerVector,hitPosition,ForceMode.Impulse);

        Vector3 fxPos = (transform.position + hitPosition) / 2;
        HitFxManager.inst.HitFX1(fxPos, power / 10);
        CameraShake.inst.Shake(0.1f, 1f);

        AudioManager.inst.PlayImpactSound(power);
    }
}
