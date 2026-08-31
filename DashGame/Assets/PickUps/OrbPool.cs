using UnityEngine;

public class OrbPool : MonoBehaviour
{
    public static OrbPool inst;
    [SerializeField] GameObject[] xpOrbs;

    private void Awake()
    {
        inst = this;
    }

    public void SpawnOrbs(int amount, Vector3 pos)
    {
        if(amount>xpOrbs.Length)amount= xpOrbs.Length;

        print("activating "+amount +" orbs...");

        for (int i = 0; i < amount; i++) 
        {
            if (!xpOrbs[i].activeInHierarchy)
            {
                xpOrbs[i].transform.position = pos;
                xpOrbs[i].gameObject.SetActive(true);
                print("activated " + xpOrbs[i].name );
            }
            else
            {
                print("cant activate " + xpOrbs[i].name);
            }
        }
    }

}
