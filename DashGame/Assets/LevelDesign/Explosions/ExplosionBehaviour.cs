using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class ExplosionBehaviour : MonoBehaviour
{
    [SerializeField] ParticleSystem[] particles;
    [SerializeField] Light explosionLight;
    [SerializeField] AudioClip explosionSound;
    [SerializeField] float explosionPower = 1f;
    SphereCollider explosionCollider;


    //private static readonly Collider[] hitBuffer = new Collider[32];

    private void OnEnable()
    {
        explosionCollider = GetComponent<SphereCollider>();
        explosionCollider.enabled = false;

    }

    private void Start() //debug test
    {
        Explode(transform.position,explosionPower);
    }

    public void Explode(Vector3 position, float power)
    {
        transform.position = position;
        foreach (var particle in particles)
        {
            particle.transform.localScale=new Vector3(power, power, power);
            particle.Play();

        }

        explosionLight.intensity = 10;

        explosionPower=power;
        explosionCollider.enabled = true;
        explosionCollider.radius = 4 * power;

       if(explosionSound!=null)AudioManager.inst.PlayCustomSound(explosionSound, explosionPower);

        Invoke("Sleep", 2.5f);

    }

    private void Update()
    {
        if(explosionLight.intensity>0)explosionLight.intensity -=(25*Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats player = other.GetComponent<PlayerStats>();
            if (player!=null)
            {
                //idk damage yet
                float playerDamage = explosionPower * 10f;

                player.TakeDamage(playerDamage,transform.position);
            }
        }

        if (other.CompareTag("Enemy"))
        {
            print("explosion hit enemy");
        }
    }

    void Sleep()
    {
        this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        foreach (var particle in particles)
        {
            particle.Stop();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,4*explosionPower);
    }
}
