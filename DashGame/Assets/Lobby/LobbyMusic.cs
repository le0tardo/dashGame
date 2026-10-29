using UnityEngine;

public class LobbyMusic : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource introSource;
    [SerializeField] private AudioSource loopSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip introClip;
    [SerializeField] private AudioClip loopClip;

    private void Start()
    {
        PlayIntroAndLoop();
    }

    public void PlayIntroAndLoop()
    {
        if (introClip == null || loopClip == null) return;

        // Assign clips
        introSource.clip = introClip;
        loopSource.clip = loopClip;

        // Ensure loop setting is correct
        introSource.loop = false;
        loopSource.loop = true;

        // Get precise DSP time reference
        double startTime = AudioSettings.dspTime;
        double introDuration = (double)introClip.samples / introClip.frequency;
        double loopStartTime = startTime + introDuration;

        // Schedule both sources
        introSource.PlayScheduled(startTime);
        loopSource.PlayScheduled(loopStartTime);
    }

    public void StopMusic()
    {
        introSource?.Stop();
        loopSource?.Stop();
    }
}
