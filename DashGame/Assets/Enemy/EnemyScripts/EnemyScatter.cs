using UnityEngine;

public class EnemyScatter : MonoBehaviour
{
    [SerializeField] float scatterPower=25f;
    private void OnEnable()
    {
        Invoke("Sleep", 0.1f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyMove em=other.gameObject.GetComponent<EnemyMove>();
            if (em != null)
            {
                em.GetHitByOtherEnemy(transform.position,scatterPower);
            }
        }
    }

    private void Sleep()
    {
        this.gameObject.SetActive(false);
    }
}
