using UnityEngine;

public class EnemyHole : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyMove em = other.GetComponent<EnemyMove>();
            if (em.isBouncing)
            {
                em.Fall(this.gameObject.transform.position);
            }
        }
    }
}
