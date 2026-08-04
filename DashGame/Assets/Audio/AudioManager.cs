using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager inst;

    [Header("Impacts")]
    [SerializeField] AudioClip[] impacts;
    [SerializeField] AudioClip[] enemyImpacts;
    [SerializeField] AudioClip[] mealImpacts;
    [Header("Misc")]
    [SerializeField] AudioClip[] swooshes;
    [Header("Hero Sounds")]
    [SerializeField] AudioClip heroFallSound;
    [SerializeField] AudioClip heroAimSound;
    [SerializeField] AudioClip heroReleaseSound;
    [SerializeField] AudioClip[] heroHurts;
    AudioSource source;
    [SerializeField] AudioSource aimSource;

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

    public void PlayCustomSound(AudioClip clip, float vol)
    {
        source.PlayOneShot(clip,vol);
    }
    public void PlayAimSound()
    {
        aimSource.Play();
    }
    public void StopAimSound()
    {
        aimSource.Stop();
    }
    public void PlayReleaseSound(float vol)
    {
        vol=Mathf.Clamp(vol, 0, 1);
        source.PlayOneShot(heroReleaseSound,(vol/4));

        int r = Random.Range(0,swooshes.Length);
        source.PlayOneShot(swooshes[r],vol);

        ResetSource();
    }

    public void PlaySwoosh(float vol)
    {
        int r = Random.Range(0, swooshes.Length);
        source.PlayOneShot(swooshes[r], vol);
    }
    public void PlayHeroFallSound()
    {
        source.volume = 0.5f;
        source.pitch = Random.Range(0.75f,1.25f);
        source.PlayOneShot(heroFallSound);

        ResetSource();
    }
    public void PlayHeroHurtSound()
    {
        int r = Random.Range(0, heroHurts.Length);
        source.PlayOneShot(heroHurts[r],0.5f);
    }

    void ResetSource()
    {
        source.volume = 1;
        source.pitch = 1;
    }
}
