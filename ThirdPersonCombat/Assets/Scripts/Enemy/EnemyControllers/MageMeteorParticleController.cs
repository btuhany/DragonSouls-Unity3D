using Combat;
using Sounds;
using System.Collections;
using UnityEngine;

public class MageMeteorParticleController : MonoBehaviour
{
    [SerializeField] private float _duration = 4f;
    [SerializeField] private int _attackDamage;
    [SerializeField] private SoundClips _spawnSFX;
    [SerializeField] private SoundClips _destroySFX;
    [SerializeField] private float _meteorSpawnDelayTime;
    private AudioSource[] _audioSource;
    private WaitForSeconds _meteorSFXdelay;

    private float _timeCounter = 0f;

    private void Awake()
    {
        _audioSource = GetComponents<AudioSource>();
        _meteorSFXdelay = new WaitForSeconds(_meteorSpawnDelayTime);
    }
    private void OnEnable()
    {
        _timeCounter = 0f;

        if (_audioSource[1].isPlaying)
            _audioSource[1].Stop();

        PlayCircleSpawnSFX();

        StopAllCoroutines();
        StartCoroutine(DelayedPlayMeteorSFX()); 
    }

    private void OnDisable()
    {
        if (_audioSource[1].isPlaying)
            _audioSource[1].Stop();
    }
    private void Update()
    {
        _timeCounter += Time.deltaTime;
        if(_timeCounter > _duration)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent(out Health health))
        {
            health.TakeDamage(_attackDamage, null);
            health.EnterHitPosition = transform.position;
        }
        PlayDestroySFX();
    }
    private void PlayDestroySFX()
    {
        _audioSource[0].volume = _destroySFX.Volume;
        _audioSource[0].pitch = _destroySFX.Pitch;
        _audioSource[0].PlayOneShot(_destroySFX.AudioClips[0]);
    }
    private void PlayCircleSpawnSFX()
    {
        _audioSource[0].volume = _spawnSFX.Volume;
        _audioSource[0].pitch = _spawnSFX.Pitch;
        _audioSource[0].PlayOneShot(_spawnSFX.AudioClips[0]);
    }
    private void PlayMeteorSFX()
    {
        _audioSource[0].volume = _spawnSFX.Volume;
        _audioSource[0].pitch = _spawnSFX.Pitch;
        _audioSource[0].PlayOneShot(_spawnSFX.AudioClips[1]);
    }
    private IEnumerator DelayedPlayMeteorSFX()
    {
        yield return _meteorSFXdelay;
        PlayMeteorSFX();
        if (!_audioSource[1].isPlaying)
            _audioSource[1].Play();
        yield return null;
    }
}
