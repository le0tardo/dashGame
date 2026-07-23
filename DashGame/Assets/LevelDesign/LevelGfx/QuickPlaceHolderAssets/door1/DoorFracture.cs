using UnityEngine;

public class DoorFracture : MonoBehaviour
{
    [SerializeField] GameObject intactParent;
    [SerializeField] GameObject fractureParent;
    [SerializeField] Rigidbody[] shardsRb;
    PlayerMove player;

    [SerializeField] bool shrinkBool=false;
    [SerializeField] float shrinkFloat=1f;
    [SerializeField] float shrinkRate = 0.1f;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerMove>();
    }
    private void Update()
    {
        if (shrinkBool)
        {
            Shrink();
        }
    }

    public void Shatter()
    {
        intactParent.SetActive(false);
        fractureParent.SetActive(true);

        foreach (Rigidbody shard in shardsRb)
        {
            Vector3 pushDir = (shard.transform.position - player.transform.position).normalized;
            pushDir.y += 2 * 0.1f;
            float shatterForce=player.currentVelocity.magnitude/10;
            shard.AddForce(pushDir * shatterForce, ForceMode.Impulse);

            /*
            if (shard.gameObject.TryGetComponent<BoxCollider>(out var col))
            {
                col.enabled = false;
            }

            //shard.isKinematic = true;
            */
        }

        shrinkBool = true;
    }

    void Shrink()
    {
        shrinkFloat -= shrinkRate * Time.deltaTime;
        foreach (var shard in shardsRb)
        {
            shard.gameObject.transform.localScale = new Vector3(shrinkFloat, shrinkFloat, shrinkFloat);
        }

        if (shrinkFloat <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
