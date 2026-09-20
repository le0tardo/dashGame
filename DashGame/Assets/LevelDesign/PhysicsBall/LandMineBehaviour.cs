using UnityEngine;

public class LandMineBehaviour : MonoBehaviour
{
    [SerializeField] float explosionPower = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")||other.CompareTag("Enemy"))
        {
            ExplosionPool.inst.SpawnExplosion(transform.position, explosionPower);
            this.gameObject.SetActive(false);
        }
    }
}
