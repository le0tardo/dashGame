using TMPro;
using UnityEngine;

public class PetProjectiles : MonoBehaviour
{
    public static PetProjectiles inst;

    [SerializeField] PetProjectile[] dragonFires;
    [SerializeField] PetProjectile[] eyeBeams;

    private void Awake()
    {
        inst = this;
    }
    public void SpawnDragonProjectile(Vector3 startPosition, Vector3 targetPosition)
    {
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

    public void SpawnEyeBeam(Vector3 startPosition, Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - startPosition).normalized;
        for (int i = 0; i < eyeBeams.Length; i++)
        {
            if (!eyeBeams[i].gameObject.activeInHierarchy)
            {
                eyeBeams[i].gameObject.transform.position = startPosition;

                if (direction != Vector3.zero)
                {
                    eyeBeams[i].transform.rotation = Quaternion.LookRotation(direction);
                }

                eyeBeams[i].gameObject.SetActive(true);
                eyeBeams[i].FireProjectile(targetPosition);
            }
        }
    }
}
