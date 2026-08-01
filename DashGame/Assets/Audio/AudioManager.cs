using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager inst;

    [SerializeField] AudioClip[] impacts;
    [SerializeField] AudioClip[] enemyImpacts;
    AudioSource source;

    private void Start()
    {
        inst= this;
        source = GetComponent<AudioSource>();
    }
    public void PlayImpactSound(float vol)
    {
        vol =(vol / 10);
        vol=Mathf.Clamp(vol, 0, 1);
        print("volume: " + vol);
        source.volume = vol;
        int r=Random.Range(0, impacts.Length);
        source.PlayOneShot(impacts[r]);
    }

    public void PlayEnemyImpactSound(float vol)
    {
        vol = (vol / 10);
        vol = Mathf.Clamp(vol, 0, 1);
        print("volume: " + vol);
        source.volume = vol;
        int r = Random.Range(0, enemyImpacts.Length);
        source.PlayOneShot(enemyImpacts[r]);

    }
}
