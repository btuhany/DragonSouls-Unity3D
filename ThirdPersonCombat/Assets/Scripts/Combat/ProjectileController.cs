using Combat;
using DG.Tweening;
using Sounds;
using System.Collections;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private int _attackDamage;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private bool _chasePlayerInitial = false;
    [SerializeField] private bool _useTween = false;
    [SerializeField] private float _tweenTime = 1f;
    [SerializeField] private Ease _tweenEase = Ease.Linear;
    [SerializeField] private float _destroyDelay = 0.5f;
    private Damage _damage;
    private Rigidbody _rb;
    public Transform targetTransform;
    private Vector3 _dir;
    [SerializeField] private ParticleSystem postDestroyFX;
    private bool _chasePlayer = false;
    private bool _isStop;
    private Tweener _movetargetAnim;
    private AudioSource[] _audioSources;
    public GameObject[] _childs;
    [Header("AudioClips")]
    [SerializeField] private SoundClips _spawnSFX;
    [SerializeField] private SoundClips _destroySFX;

    private void Awake()
    {
        postDestroyFX.Stop();
        _damage = GetComponent<Damage>();
        _rb = GetComponent<Rigidbody>();
        _audioSources = GetComponents<AudioSource>();
    }
    private void OnEnable()
    {
        SetChildObjects(true);
        PlaySpawnSFX();
        _chasePlayer = _chasePlayerInitial;
        _damage.OnHitGiven += HandleOnDamageGiven;

        if(_useTween)
        {
            _isStop = true;
            Vector2 randomVec = Random.onUnitSphere * 1.3f;
            transform.position += transform.right * randomVec.x + transform.up * randomVec.y;
            _movetargetAnim = _rb.DOMove(targetTransform.position, _tweenTime).SetEase(_tweenEase);
            _movetargetAnim.onComplete = () => { _isStop = false; };
        }
        else
        {
            _isStop = false;
        }
        _dir = targetTransform.position - _rb.position;
    }
    private void OnDisable()
    {
        _damage.ResetState();
        _damage.OnHitGiven -= HandleOnDamageGiven;
    }
    private void Start()
    {
        _damage.AttackDamage = _attackDamage;
    }
    private void FixedUpdate()
    {
        if (_isStop) return;
        if (_chasePlayer)
            _dir = targetTransform.position - _rb.position;
        _rb.MovePosition(_rb.position + _dir.normalized * Time.fixedDeltaTime * _speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_chasePlayer)
                _chasePlayer = false;
            return;
        }

        DisableProjectile();
    }
    private void HandleOnDamageGiven(Collider other)
    {
        DisableProjectile();
    }
    private void DisableProjectile()
    {
        if (_useTween)
            _movetargetAnim.Pause();
        postDestroyFX.Play();
        _isStop = true;
        SetChildObjects(false);
        PlayDestroySFX();
        Destroy(this.gameObject, _destroyDelay);
    }
    private void PlaySpawnSFX()
    {
        _audioSources[1].volume = _spawnSFX.Volume;
        _audioSources[1].pitch = _spawnSFX.Pitch;
        _audioSources[1].PlayOneShot(_spawnSFX.AudioClips[0]);
    }
    private void PlayDestroySFX()
    {
        _audioSources[1].volume = _destroySFX.Volume;
        _audioSources[1].pitch = _destroySFX.Pitch;
        _audioSources[1].PlayOneShot(_destroySFX.AudioClips[0]);
    }
    private void SetChildObjects(bool active) //Used before destroying the object or respawning.
    {
        foreach (GameObject child in _childs)
        {
            child.gameObject.SetActive(active);
        }
    }
}
