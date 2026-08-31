using UnityEngine;

public class HitFxManager : MonoBehaviour
{

    public static HitFxManager inst;
    [SerializeField] ParticleSystem[] hitFX1;
    [SerializeField] ParticleSystem[] pickupXpFx;
    [SerializeField] ParticleSystem[] fireHitFx;

    private void Awake()
    {
        inst=this;
    }
    public void HitFX1(Vector3 pos, float scl)
    {
        for (int i = 0; i < hitFX1.Length; i++)
        {
            if (!hitFX1[i].gameObject.activeInHierarchy)
            {
                hitFX1[i].gameObject.transform.position = pos;
                hitFX1[i].gameObject.transform.localScale = Vector3.one*(scl/5); //just looked better...

                hitFX1[i].gameObject.SetActive(true);
                hitFX1[i].Play();
                return;
            }
        }
    }

    public void PickupXpFx(Vector3 pos)
    {
        for (int i = 0; i < pickupXpFx.Length; i++)
        {
            if (!pickupXpFx[i].gameObject.activeInHierarchy)
            {
                pickupXpFx[i].gameObject.transform.position = pos;
                pickupXpFx[i].gameObject.SetActive(true);
                pickupXpFx[i].Play();

                ParticleSystem childFx = null;

                foreach (Transform child in pickupXpFx[i].transform)
                {
                    childFx = child.GetComponentInChildren<ParticleSystem>();
                    if (childFx != null) break;
                }

                if (childFx != null) childFx.Play();

                return;
            }
        }
    }

    public void FireHitFx(Vector3 pos, Quaternion rot)
    {
        for(int i = 0; i < fireHitFx.Length; i++)
        {
            if (!fireHitFx[i].gameObject.activeInHierarchy)
            {
                fireHitFx[i].gameObject.transform.position = pos;
                fireHitFx[i].gameObject.transform.rotation = rot;
                fireHitFx[i].gameObject.SetActive(true);
                fireHitFx[i].Play();

                ParticleSystem childFx = null;

                foreach (Transform child in fireHitFx[i].transform)
                {
                    childFx = child.GetComponentInChildren<ParticleSystem>();
                    if (childFx != null) break;
                }

                if (childFx != null) childFx.Play();

                return;
            }
        }
    }
}
