using UnityEngine;

public class ZombieShards : MonoBehaviour
{
    [SerializeField] Rigidbody[] rbs;
    Transform player;

    private void Start()
    {
        player = LevelManager.inst.playerMove.gameObject.transform;

        foreach (var rb in rbs) 
        {
            Vector3 playerVelocity = LevelManager.inst.playerMove.currentVelocity;
            rb.AddForceAtPosition(playerVelocity,player.position,ForceMode.Impulse);
            print("added force: " + playerVelocity +". total: "+playerVelocity.magnitude);
        }

        Invoke("DisableRB", 1.5f);
    }

    void DisableRB()
    {
        foreach (var rb in rbs)
        {
            rb.isKinematic = false;
        }
    }
}
