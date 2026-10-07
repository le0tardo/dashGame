using UnityEngine;

public class WallShotBehaviour : MonoBehaviour
{

    Vector3 restPosition;
    [SerializeField] float speed=20f;
    [SerializeField] float damage=10f;
    public bool fired = false;
    [SerializeField] LayerMask hittableLayer;
    [SerializeField] TrailRenderer trail;

    [Header("HitFx")]
    [SerializeField] GameObject hitParticle;
    [SerializeField] ParticleSystem[] fx;

    [Header("Sounds")]
    [SerializeField] AudioClip fireSound;
    [SerializeField] AudioClip hitSound;

    private void Start()
    {
        restPosition = transform.localPosition;
    }

    public void FireArrow()
    {
        if (!fired)
        {
            fired = true;
            AudioManager.inst.PlayCustomSound(fireSound, 1f);
            trail.emitting = true;
        }
    }

    void StopArrow()
    {
        fired = false;
        trail.emitting = false;

        hitParticle.transform.position=transform.position;
        foreach (var f in fx)
        {
            f.Play();
        }

        AudioManager.inst.PlayCustomSound(hitSound, 1f);

        transform.localPosition = restPosition;
    }

    private void Update()
    {
        if (fired)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats player =other.GetComponent<PlayerStats>();
           if(player!=null) player.TakeLightDamage(damage, transform.position);
            StopArrow();

        }
        if (other.CompareTag("Enemy"))
        {
            EnemyCombat enemy = other.GetComponent<EnemyCombat>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                StopArrow();
            }
        }

        if(other.TryGetComponent<IHittable>(out var target))
        {
            print("arrow hit obstacle");
            StopArrow();
        }

        if (((1 << other.gameObject.layer) & hittableLayer) != 0)
        {
            print("arrow hit wall");
            StopArrow();
        }
    }
}
