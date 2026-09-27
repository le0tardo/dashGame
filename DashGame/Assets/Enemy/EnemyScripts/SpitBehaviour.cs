using UnityEngine;

public class SpitBehaviour : MonoBehaviour
{
    [SerializeField] float projectileSpeed;
    [SerializeField] float projectileDamage;
    [SerializeField] AudioClip spitSpawnSound;
    [SerializeField] AudioClip spitHitSound;

    private void OnEnable()
    {
        AudioManager.inst.PlayCustomSound(spitSpawnSound,0.5f);
    }

    private void Update()
    {
        transform.position += transform.forward * projectileSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats player = other.GetComponent<PlayerStats>();
            if (player != null)
            {
                player.TakeLightDamage(projectileDamage,transform.position);
            }

            SpitPool.inst.SpitHit(transform.position, transform.rotation);
            AudioManager.inst.PlayCustomSound(spitHitSound, 0.5f);

            this.gameObject.SetActive(false);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Room"))
        {
            SpitPool.inst.SpitHit(transform.position, transform.rotation);
            AudioManager.inst.PlayCustomSound(spitHitSound, 0.5f);
            this.gameObject.SetActive(false);
        }
    }
}
