using DG.Tweening;
using UnityEngine;
using Combat;
using Sounds;
using States;

namespace PlayerController
{
    public class Sword : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _freezeElectricFX;

        [SerializeField] private float _curvePointToHolderReturnLerpTime = 0.3f;
        [SerializeField] private Transform _curvePoint;
        [SerializeField] private Transform _handHolder;
        [SerializeField] private Transform _sheahtHolder;
        [SerializeField] private float _curvePointReachingSpeed = 10f;
        [SerializeField] private float _targetReachingSpeed = 20f;
        [SerializeField] private int _damageInAir = 25;

        [Header("Sounds")]
        [SerializeField] private SoundClips _swordDamageSounds;
        [SerializeField] private SoundClips _swordDraw;
        [SerializeField] private SoundClips _swordSheath;
        [SerializeField] private SoundClips _swordSwipe;
        [SerializeField] private SoundClips _swordGrab;
        [SerializeField] private SoundClips _groundHitImpact;


        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioSource _inAirSFXAuidoSource;
        [SerializeField] private AudioSource _returnSFXAuidoSource;
        
        [Header("Sword Throw")]
        [SerializeField] private LayerMask _aimLayer;
        [SerializeField] private float _aimRange;
        private TrailRenderer _trail;
        private float _curvePointReachingTime = 1.0f;
        private bool _isReturning = false;
        private bool _isOnThrow = false;
        private bool _isInCurvePoint = false;
        private bool _isOnEnemy = false;
        private bool _isOnAgent = false;
        private AiAgent _currentAgent;
        private Rigidbody _rb;
        private Tweener _zRotationAnim;
        private Tweener _yRotationAnim;
        private Tweener _throwTween;
        private CapsuleCollider _collider;
        private Damage _damage;
        private Transform _mainCam;
        private Transform _swordBody;
        private EnemyStateMachine _currentEnemy;
        private float _inAirSFXInitialVolume;
        private float _returnSFXInitialVolume;
        public bool IsInHand => transform.parent == _handHolder;
        public bool IsInSheath => transform.parent == _sheahtHolder;

        private void Awake()
        {
            _inAirSFXInitialVolume = _inAirSFXAuidoSource.volume;
            _returnSFXInitialVolume = _returnSFXAuidoSource.volume;
            _trail = GetComponentInChildren<TrailRenderer>();
            _swordBody = GetComponentsInChildren<Transform>()[1];
            _damage = GetComponent<Damage>();
            _rb = GetComponent<Rigidbody>();
            _collider = GetComponent<CapsuleCollider>();
            _audioSource = GetComponent<AudioSource>();
            _zRotationAnim = transform.DORotate(new Vector3(0f, 0f, -180f), 0.1f).SetLoops(-1, LoopType.Incremental);
            _yRotationAnim = _swordBody.DOLocalRotate(new Vector3(0f, -180f, 0f), 0.1f).SetLoops(-1, LoopType.Incremental);
            _yRotationAnim.Pause();
            _zRotationAnim.Pause();
            _damage.enabled = false;
            _mainCam = Camera.main.transform;
        }
        private void OnEnable()
        {
            _collider.enabled = false;
            _trail.enabled = false;
            _freezeElectricFX.gameObject.SetActive(false);
        }
        private void FixedUpdate()
        {
            if (_isInCurvePoint)
            {
                if (_inAirSFXAuidoSource.isPlaying)
                    SetActiveInAirSFX(false);

                Vector3 dir = Vector3.Slerp(transform.position, _handHolder.position, _curvePointToHolderReturnLerpTime);
                _rb.MovePosition(dir);
                if (Vector3.Distance(_rb.position, _handHolder.position) < 1f)
                {
                    SetActiveReturnSFX(false);
                    PlayGrabSFX();
                    StopAttack();
                    _zRotationAnim.Pause();
                    _yRotationAnim.Pause();
                    _isInCurvePoint = false;
                    _isReturning = false;
                    _rb.isKinematic = true;
                    _collider.enabled = false;
                    transform.SetParent(_handHolder);
                    _rb.velocity = Vector3.zero;
                    transform.localPosition = Vector3.zero;
                    transform.localRotation = Quaternion.Euler(Vector3.zero);
                }
            }
            if(!_isReturning && _isOnThrow)
            {
                float distance = Vector3.Distance(_rb.position, PlayerStateMachine.Instance.transform.position);
                if(distance >= 250f) //max distance
                {
                    _isOnThrow = false;
                    _rb.velocity = Vector3.zero;
                    _yRotationAnim.Pause();
                    _rb.isKinematic = true;
                    SetActiveInAirSFX(false);
                }
            }
                
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Trigger")) return;
            if (other.CompareTag("Bonfire")) return;
            if (!_isReturning && _isOnThrow)
            {
                _isOnThrow = false;
                _rb.velocity = Vector3.zero;
                _yRotationAnim.Pause();
                _rb.isKinematic = true;
                //transform.SetParent(other.transform);
                SetActiveInAirSFX(false);
                if (other.CompareTag("Enemy"))
                {
                    if(other.TryGetComponent(out EnemyStateMachine enemy))
                    {
                        transform.SetParent(other.transform);
                        _currentEnemy = enemy;
                        if(!_currentEnemy.isOnDeadState && !_currentEnemy.isDead)
                            _currentEnemy.ChangeState(_currentEnemy.swordHitState);
                        _currentEnemy.isSwordOn = true;
                        _currentEnemy.sword = this;
                        _isOnEnemy = true;
                        _freezeElectricFX.gameObject.SetActive(true);
                    }
                    else if(other.TryGetComponent(out AiAgent agent))
                    {
                        transform.SetParent(other.transform);
                        agent.isSwordOnThis = true;
                        agent.sword = this;
                        _isOnAgent = true;
                        _currentAgent = agent;
                    }
                }
                else
                {
                    PlayGroundHitImpactSFX();
                }
            }
        }
        private void CalculateReturnTime()
        {
            float distance = Vector3.Distance(_curvePoint.position, _rb.transform.position);
            _curvePointReachingTime = distance / _curvePointReachingSpeed;
        }
        public void Throwed(Vector3 force, Transform player)
        {
            _isOnThrow = true;
            _collider.enabled = true;
            _rb.isKinematic = false;
            transform.parent = null;
            //transform.localRotation = Quaternion.Euler(Vector3.zero);
            transform.rotation = Quaternion.LookRotation(player.right, _mainCam.transform.forward);
            _yRotationAnim.Play();
            StartAttack(_damageInAir);
            ThrowProcess(force);
            SetActiveInAirSFX(true);
        }
        public void OnEnemyDeath()
        {
            transform.position = _currentEnemy.transform.position;
        }
        private void ThrowProcess(Vector3 force)
        {
            if(Physics.Raycast(_mainCam.position, _mainCam.forward, out RaycastHit hit, _aimRange, _aimLayer))
            {
                float inAirTime = Vector3.Distance(hit.point, transform.position) / _targetReachingSpeed;
                _throwTween = _rb.DOMove(hit.point, inAirTime);
                _throwTween.onComplete = () =>
                {
                    if(_isOnThrow)
                        _rb.AddForce(force, ForceMode.Impulse);
                };
                if (!_throwTween.IsPlaying())
                    _throwTween.Play();
            }
            else
            {
                _rb.AddForce(force, ForceMode.Impulse);
            }
        }
        public void Return()
        {
            if (!_returnSFXAuidoSource.isPlaying)
                SetActiveReturnSFX(true);
            if (_inAirSFXAuidoSource.isPlaying)
                SetActiveInAirSFX(false);

            if (_isOnEnemy)
            {
                if(!_currentEnemy.isDead || !_currentEnemy.isOnDeadState)
                    _currentEnemy.ChangeState(_currentEnemy.chaseState);
                _freezeElectricFX.gameObject.SetActive(false);
                _currentEnemy.isSwordOn = false;
                _isOnEnemy = false;
            }
            if (_isOnAgent)
            {
                _currentAgent.isSwordOnThis = false;
                _currentAgent.sword = null;
                _isOnAgent = false;
            }
            transform.parent = null;
            if(_isOnThrow)
            {
                _isOnThrow = false;
                _rb.velocity = Vector3.zero;
                _yRotationAnim.Pause();
                _throwTween.Pause();
            }
            CalculateReturnTime();
            _isReturning = true;
            _rb.isKinematic = false;
            _rb.velocity = Vector3.zero;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            _zRotationAnim.Play();
            _rb.DOMove(_curvePoint.position, _curvePointReachingTime).SetEase(Ease.InCubic).onComplete = () =>
            {
                _isInCurvePoint = true;
            };
        }
        public void Unsheath()
        {
            transform.SetParent(_handHolder);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            PlayDrawSFX();
        }

        public void Sheath()
        {
            transform.SetParent(_sheahtHolder);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            PlaySheathSFX();
        }

        public void StartAttack(int damage)
        {
            _trail.enabled = true;
            _collider.enabled = true;
            _damage.ResetState();
            _damage.SetAttackDamage(damage);
            _damage.enabled = true;
            PlaySwipeSFX();
        }
        
        public void StopAttack()
        {
            _collider.enabled = false;
            _damage.enabled = false;
            _trail.enabled = false;
        }
        public void DetachFromAgent()
        {
            transform.SetParent(null);
        }
        public void DetachFromEnemy()
        {
            _freezeElectricFX.gameObject.SetActive(false);
            _currentEnemy.isSwordOn = false;
            _isOnEnemy = false;
            transform.SetParent(null);
        }

        #region Sounds
        public void PlayDamageGivenSFX()
        {
            _audioSource.volume = _swordDamageSounds.Volume;
            _audioSource.pitch = _swordDamageSounds.Pitch;
            _audioSource.PlayOneShot(_swordDamageSounds.AudioClips[Random.Range(0, _swordDamageSounds.AudioClips.Length)]);
        }
        private void PlayDrawSFX()
        {
            _audioSource.volume = _swordDraw.Volume;
            _audioSource.pitch = _swordDraw.Pitch;
            _audioSource.PlayOneShot(_swordDraw.AudioClips[0]);
        }
        private void PlaySheathSFX()
        {
            _audioSource.volume = _swordSheath.Volume;
            _audioSource.pitch = _swordSheath.Pitch;
            _audioSource.PlayOneShot(_swordSheath.AudioClips[0]);
        }
        private void PlaySwipeSFX()
        {
            _audioSource.volume = _swordSwipe.Volume;
            _audioSource.pitch = _swordSwipe.Pitch + Random.Range(-0.24f, 0.24f);
            _audioSource.PlayOneShot(_swordSwipe.AudioClips[0]);
        }
        private void PlayGrabSFX()
        {
            _audioSource.volume = _swordGrab.Volume;
            _audioSource.pitch = _swordGrab.Pitch + Random.Range(-0.24f, 0.24f);
            _audioSource.PlayOneShot(_swordGrab.AudioClips[0]);
        }
        private void PlayGroundHitImpactSFX()
        {
            _audioSource.volume = _groundHitImpact.Volume;
            _audioSource.pitch = _groundHitImpact.Pitch + Random.Range(-0.5f, 0.5f);
            _audioSource.PlayOneShot(_groundHitImpact.AudioClips[0]);
        }
        private void SetActiveInAirSFX(bool active)
        {
            if(active)
            {
                _inAirSFXAuidoSource.Play();
            }
            else
            {
                _inAirSFXAuidoSource.DOFade(0.0f, 0.2f).onComplete = () => {
                    _inAirSFXAuidoSource.Stop();
                    _inAirSFXAuidoSource.volume = _inAirSFXInitialVolume;
                };
            }
        }
        private void SetActiveReturnSFX(bool active)
        {
            if (active)
            {
                _returnSFXAuidoSource.Play();
            }
            else
            {
                _returnSFXAuidoSource.DOFade(0.0f, 0.2f).onComplete = () =>
                {
                    _returnSFXAuidoSource.Stop();
                    _returnSFXAuidoSource.volume = _returnSFXInitialVolume;
                };
            }
        }
        #endregion
    }

}
