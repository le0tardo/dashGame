using UnityEngine;

public class ExplosionPool : MonoBehaviour
{
    public static ExplosionPool inst;
    [SerializeField] ExplosionBehaviour[] explosions;
    private void Awake()
    {
        inst = this;
    }

    public void SpawnExplosion(Vector3 position, float power)
    {
        for (int i = 0; i < explosions.Length; i++)
        {
            if (!explosions[i].isActiveAndEnabled)
            {
                explosions[i].gameObject.SetActive(true);
                explosions[i].Explode(position, power);
                return;
            }
        }
    }
}
