using System.Collections;
using UnityEngine;

public class ZombieShards : MonoBehaviour
{
    [SerializeField] Rigidbody[] rbs;
    [SerializeField] GameObject[] chunks;
    float shrink = 1f;
    Transform player;

    [SerializeField] Coroutine sinkRoutine;
    private void Awake()
    {
        player = LevelManager.inst.playerMove.gameObject.transform;

        foreach (var rb in rbs) 
        {
            Vector3 playerVelocity = LevelManager.inst.playerMove.currentVelocity;
            rb.AddForceAtPosition(playerVelocity,player.position,ForceMode.Impulse);
        }
    }

    private void Update()
    {
        shrink-=Time.deltaTime;
        foreach (var chunk in chunks)
        {
            chunk.transform.localScale = new Vector3(shrink, shrink, shrink);
        }

        if (shrink <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }


}
