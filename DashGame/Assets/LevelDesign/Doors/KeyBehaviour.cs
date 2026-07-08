using UnityEngine;

public class KeyBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LevelManager.inst.keys++;
            Destroy(this.gameObject);
        }
    }
}
