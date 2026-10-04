using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LevelManager.inst.SetCheckPoint(transform.position);
        }
    }
}
