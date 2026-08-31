using UnityEngine;
using UnityEngine.VFX;

public class VolleyBullet : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float lifetime=5f;
    [SerializeField] float damage = 1f;
    [SerializeField] AudioClip fireHit;
    float lifeTimer;

    Vector3 moveDirection;

    BulletVolley volley;

    private void Awake()
    {
        volley = GetComponentInParent<BulletVolley>();
    }

    public void Launch(Vector3 direction, float bulletSpeed)
    {
        moveDirection=direction.normalized;
        lifeTimer = lifetime;
        speed = bulletSpeed;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        transform.position += moveDirection * (speed * Time.deltaTime);

        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            PlayerStats player= other.GetComponent<PlayerStats>();
            if (player != null)
            {
                player.TakeLightDamage(damage,transform.position);

                Vector3 hitPos = (transform.position + player.gameObject.transform.position) / 2f;

                HitFxManager.inst.FireHitFx(hitPos,transform.rotation);
                if (fireHit != null) AudioManager.inst.PlayCustomSound(fireHit, 1f);
            }

            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Room"))
        {
            HitFxManager.inst.FireHitFx(transform.position, transform.rotation);
            this.gameObject.SetActive(false);
        }
    }
}
