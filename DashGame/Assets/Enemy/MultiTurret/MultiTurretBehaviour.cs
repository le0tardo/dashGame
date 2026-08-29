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

    [SerializeField] float bounce = 0.5f;
    public float hitBounce => bounce;

    private void Awake()
    {
        ammoPool = FindObjectsByType<BulletVolley>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        InvokeRepeating("FireVolley",1f,repeatRate);

        maxHealth = health;
    }
    void FireVolley()
    {
        print("trying to fire volley");
        if (ammoPool.Length > 0)
        {
            for (int i = 0; i < ammoPool.Length; i++)
            {
                if (!ammoPool[i].gameObject.activeInHierarchy)
                {
                    ammoPool[i].gameObject.SetActive(true);
                    ammoPool[i].FireVolley(bulletAmount,this.gameObject.transform.position);
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
        AudioManager.inst.PlayMetalImpactSound(power);
        

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        CancelInvoke();
        this.gameObject.SetActive(false);
    }
}
