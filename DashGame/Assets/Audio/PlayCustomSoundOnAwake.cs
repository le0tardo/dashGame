using UnityEngine;

public class PlayCustomSoundOnAwake : MonoBehaviour
{
    [SerializeField] AudioClip customSound;
    [SerializeField] float customVolume=0.5f;

    private void OnEnable()
    {
        if (AudioManager.inst != null)
        {
            AudioManager.inst.PlayCustomSound(customSound,customVolume);
        }
    }
}
