using Sounds;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundClips[] _audioClips;
    private AudioSource _audioSource;
    public static SoundManager Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        _audioSource = GetComponent<AudioSource>();
    }
    public void PlayAuidoClip(int soundIndex, int clipIndex)
    {
        ConfigAudioSourceForSound(soundIndex);
        _audioSource.PlayOneShot(_audioClips[soundIndex].AudioClips[clipIndex]);
    }
    private void ConfigAudioSourceForSound(int soundIndex)
    {
        _audioSource.volume = _audioClips[soundIndex].Volume;
        _audioSource.pitch = _audioClips[soundIndex].Pitch;
    }

}
