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
            if (other.TryGetComponent<EnemyMove>(out var hitEnemy))
            {
                Vector3 pushDirection = other.transform.position - transform.position;
                hitEnemy.GetHitByOtherEnemy(pushDirection, scatterPower);
                print("scattercollider activated + found and enemy to collide with! :))");
            }
        }
    }

    private void Sleep()
    {
        this.gameObject.SetActive(false);
    }
}
