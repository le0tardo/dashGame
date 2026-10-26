using UnityEngine;

public class PetProjectiles : MonoBehaviour
{
    public static PetProjectiles inst;

    [SerializeField] PetProjectile[] dragonFires;

    private void Awake()
    {
        inst = this;
    }
    public void SpawnDragonProjectile(Vector3 startPosition, Vector3 targetPosition)
    {
        //activate first in array
        print("attack from projectile pool");

        for (int i = 0; i < dragonFires.Length; i++)
        {
            if (!dragonFires[i].gameObject.activeInHierarchy)
            {
                dragonFires[i].gameObject.transform.position= startPosition;
                dragonFires[i].gameObject.SetActive(true);
                dragonFires[i].FireProjectile(targetPosition);
            }
        }
    }
}
