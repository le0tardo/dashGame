using UnityEngine;

public class VolleyBullet : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float lifetime=5f;
    [SerializeField] float damage = 1f;
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
            print("turret bullet on player");

            PlayerStats player= other.GetComponent<PlayerStats>();
            if (player != null)
            {
                player.TakeLightDamage(damage,transform.position);
            }

            this.gameObject.SetActive(false);
        }
    }
}
