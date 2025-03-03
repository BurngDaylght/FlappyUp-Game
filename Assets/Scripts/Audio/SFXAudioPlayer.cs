using UnityEngine;

public class SFXAudioPlayer : MonoBehaviour
{
    public static SFXAudioPlayer instance;
    
    [SerializeField] private AudioSource _audioSource;

    private void Awake()
    {
        instance = this;
    }

    public void PlaySFX(AudioClip clip, float volume = 1.0f)
    {
        if (Settings.instance.isSfxEnabled)
        {
            if (clip == null) return;
            _audioSource.PlayOneShot(clip, Mathf.Clamp01(volume));
        }
    }
}
