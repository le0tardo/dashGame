using UnityEngine;

public class HoleBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            PlayerMove pl=other.GetComponent<PlayerMove>();
            pl.Fall(this.gameObject.transform.position);
        }
        if (other.CompareTag("Enemy"))
        {
            EnemyMove em = other.GetComponent<EnemyMove>();
            em.Fall(this.gameObject.transform.position);
        }
    }
}
