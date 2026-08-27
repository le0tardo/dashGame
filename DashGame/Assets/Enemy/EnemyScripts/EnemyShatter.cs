using UnityEngine;

public class EnemyShatter : MonoBehaviour
{
    [SerializeField] Rigidbody[] rbs;
    [SerializeField] GameObject[] shards;

    [SerializeField] float shrink = 1f;


    private void Awake()
    {
        //debug
        //Vector3 testForce =new Vector3 (5, 5, 5);
        //Shatter(transform.position, testForce);
    }

    public void Shatter(Vector3 impactPostion,Vector3 impactForce)
    {
        transform.position = impactPostion; //move pooled obj to impact position

        foreach (var rb in rbs)
        {
            rb.AddForceAtPosition(impactForce, impactPostion, ForceMode.Impulse);
        }

        Invoke("Freeze", 2f);
    }

    private void Update()
    {
        if (shrink > 0)
        {
            shrink -= Time.deltaTime/4;

            foreach (var shard in shards)
            {
                shard.transform.localScale = new Vector3(shrink, shrink, shrink);
            }
        }
        else
        {
            ResetShatter();
        }
    }

    void Freeze()
    {
        foreach (var rb in rbs)
        {
            rb.isKinematic = true;
        }
    }

    void ResetShatter()
    {
        transform.position = Vector3.zero;
        //reset all shards in shard to their respective starting position and 1,1,1 scale.
        shrink = 1f;
        this.gameObject.SetActive(false);
    }
}
