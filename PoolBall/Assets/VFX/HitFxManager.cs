using UnityEngine;

public class HitFxManager : MonoBehaviour
{

    public static HitFxManager inst;
    [SerializeField] ParticleSystem[] hitFX1;

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
                hitFX1[i].gameObject.transform.localScale = Vector3.one*(scl/10);
                hitFX1[i].gameObject.SetActive(true);
                hitFX1[i].Play();
                return;
            }
        }
    }
}
