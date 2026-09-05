using UnityEngine;

public class PhysicsBallBehaviour : MonoBehaviour,IHittable
{
    [SerializeField] float bounce = 0.85f;
    public float hitBounce => bounce;

    [SerializeField] AudioClip hitSound;

    [SerializeField] Transform player;

    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = LevelManager.inst.playerMove.gameObject.transform;
    }
    public void OnHit(Vector3 hitPosition, float power)
    {
        player = LevelManager.inst.playerMove.gameObject.transform;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Vector3 powerVector = new Vector3(power,0,power);
        Vector3 flatPosition=new Vector3(hitPosition.x,transform.position.y,hitPosition.z);
        rb.AddForceAtPosition(powerVector,flatPosition,ForceMode.Impulse);

        Vector3 fxPos = (transform.position + hitPosition) / 2;
        HitFxManager.inst.HitFX1(fxPos, power / 10);
        CameraShake.inst.Shake(0.1f, 1f);

       if(hitSound!=null) AudioManager.inst.PlayCustomSound(hitSound,power/50);
    }
}
