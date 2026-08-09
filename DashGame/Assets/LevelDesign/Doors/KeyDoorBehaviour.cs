using UnityEngine;

public class KeyDoorBehaviour : MonoBehaviour,IHittable
{
    [SerializeField] Animator anim;
    public float bounce = 0.85f;
    public float hitBounce=>bounce;

    [SerializeField] AudioClip tink;
    [SerializeField] AudioClip unlock;
    [SerializeField] GameObject particle;
    BoxCollider box;

    private void Awake()
    {
        box = GetComponent<BoxCollider>();
    }
    public void OnHit(Vector3 hitPosition, float power)
    {
        if (LevelManager.inst.keys > 0)
        {
            Open();
        }
        else
        {
            anim.SetTrigger("wobble");
            AudioManager.inst.PlayCustomSound(tink, 1f);
            Vector3 fxPos = (transform.position + hitPosition) / 2;
            HitFxManager.inst.HitFX1(fxPos, power / 10);
            CameraShake.inst.Shake(0.1f, 1f);

        }
    }

    void Open()
    {
        LevelManager.inst.keys--;
        box.enabled = false;
        anim.SetTrigger("open");
        particle.SetActive(true);
        AudioManager.inst.PlayCustomSound(unlock, 1f);
        Invoke("Kill",0.5f);
    }

    void Kill()
    {
        this.gameObject.SetActive(false);
    }
}
