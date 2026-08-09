using UnityEngine;

public class DoorWoodBehaviour : MonoBehaviour, IHittable
{
    [SerializeField] float durability;
    [SerializeField] float bounce = 0.85f;
    public float hitBounce => bounce;

    [SerializeField] GameObject intact;
    [SerializeField] GameObject broken;

    [SerializeField] Animator anim;
    [SerializeField] AudioClip doorHit;
    [SerializeField] AudioClip doorBreak;
    public void OnHit(Vector3 hitPosition, float power)
    {
        durability-=power;
        print("door took" + power + " hit, remaining durability: " + durability);
        if (durability <= 0)
        {
            BreakDoor(hitPosition, power);
            AudioManager.inst.PlayCustomSound(doorBreak, 0.8f);
        }
        else
        {
            AudioManager.inst.PlayCustomSound(doorHit, 0.8f);
            anim.SetTrigger("wobble");
        }
        Vector3 fxPos = (transform.position + hitPosition) / 2;
        HitFxManager.inst.HitFX1(fxPos, power / 10);
        CameraShake.inst.Shake(0.1f, 1f);
    }

    void BreakDoor(Vector3 pos, float pwr)
    {
        broken.SetActive(true);
        DoorFracture df=broken.GetComponent<DoorFracture>();
        df.Shatter();
        intact.SetActive(false); //<. this last
    }
}
