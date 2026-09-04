using UnityEngine;

public class MultiTurretBehaviour : MonoBehaviour, IHittable
{
    [Header("Fire settings")]
    [SerializeField] private BulletVolley[] ammoPool;
    [SerializeField] int bulletAmount=5;
    [SerializeField] float repeatRate=2f;

    [Header("Health")]
    [SerializeField] float health;
    float maxHealth;
    [SerializeField] int xp;

    [SerializeField] float bounce = 0.5f;
    public float hitBounce => bounce;

    [SerializeField] Animator anim;
    [SerializeField] FlashRed flash;
    [SerializeField] AudioClip turretHit;
    [SerializeField] AudioClip turretDie;
    [SerializeField] AudioClip turretFire;
    [SerializeField] AudioClip turretAwake;
    [SerializeField] GameObject muzzleFlash;
    private void Awake()
    {
        ammoPool = FindObjectsByType<BulletVolley>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        flash=GetComponent<FlashRed>();

        maxHealth = health;
    }
    private void Start()
    {
       if(AudioManager.inst!=null) AudioManager.inst.PlayCustomSound(turretAwake, 1f);
    }
    void FireVolley()
    {
        muzzleFlash.SetActive(false);
        if (ammoPool.Length > 0)
        {
            for (int i = 0; i < ammoPool.Length; i++)
            {
                if (!ammoPool[i].gameObject.activeInHierarchy)
                {
                    ammoPool[i].gameObject.SetActive(true);
                    ammoPool[i].FireVolley(bulletAmount,this.gameObject.transform.position);
                    muzzleFlash.SetActive(true);

                    if (anim != null) anim.SetTrigger("fire");
                    AudioManager.inst.PlayCustomSound(turretFire, 1f);

                    return;
                }
            }
        }
    }

    public void OnHit(Vector3 hitPosition, float power)
    {
        float dmg = Mathf.Floor(power/10);
        health -= dmg;
        
        Vector3 fxPos = (transform.position + hitPosition) / 2;
        HitFxManager.inst.HitFX1(fxPos, power / 10);
        CameraShake.inst.Shake(0.1f, 1f);
        AudioManager.inst.PlayCustomSound(turretHit, dmg/2);


        if (anim != null) anim.SetTrigger("hurt");
        if(flash!=null)flash.Flash();

        if (health <= 0)
        {
            ShatterManager.inst.ShatterMurret1(transform.position);
            AudioManager.inst.PlayCustomSound(turretDie, 1);
            Die();
        }
    }

    private void OnEnable()
    {
        InvokeRepeating("FireVolley", 1f, repeatRate);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    void Die()
    {
        CancelInvoke();
        OrbPool.inst.SpawnOrbs(xp,transform.position);
        this.gameObject.SetActive(false);
    }
}
