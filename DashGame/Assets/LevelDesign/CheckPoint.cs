using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("Set player checkpoint");
            LevelManager.inst.SetCheckPoint(transform.position);
        }
    }
}
