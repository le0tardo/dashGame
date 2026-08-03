using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager inst;

    [SerializeField] AudioClip[] impacts;
    [SerializeField] AudioClip[] enemyImpacts;
    [SerializeField] AudioClip[] mealImpacts;
    [SerializeField] AudioClip heroFallSound;
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
        source.volume = vol;
        int r=Random.Range(0, impacts.Length);
        source.PlayOneShot(impacts[r]);
    }

    public void PlayEnemyImpactSound(float vol)
    {
        vol = (vol / 10);
        source.volume = vol-0.5f;
        vol = Mathf.Clamp(vol, 0, 1);
        int r = Random.Range(0, enemyImpacts.Length);
        source.PlayOneShot(enemyImpacts[r]);
    }

    public void PlayMetalImpactSound(float vol)
    {
        vol = (vol / 10);
        source.volume = vol - 0.5f;
        vol = Mathf.Clamp(vol, 0, 1);
        int r = Random.Range(0, mealImpacts.Length);
        source.PlayOneShot(mealImpacts[r]);
    }

    public void PlayCustomSound(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }

    public void PlayHeroFallSound()
    {
        source.volume = 0.5f;
        source.pitch = Random.Range(0.75f,1.25f);
        source.PlayOneShot(heroFallSound);

        ResetSource();
    }

    void ResetSource()
    {
        source.volume = 1;
        source.pitch = 1;
    }
}
