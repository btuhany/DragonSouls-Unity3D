using DG.Tweening;
using UnityEngine;
using Combat;
using Sounds;

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

        [Header("Sword Throw")]
        [SerializeField] private LayerMask _aimLayer;
        [SerializeField] private float _aimRange;
        private TrailRenderer _trail;
        private float _curvePointReachingTime = 1.0f;
        private bool _isReturning = false;
        private bool _isOnThrow = false;
        private bool _isInCurvePoint = false;
        private bool _isOnEnemy = false;
        private Rigidbody _rb;
        private Tweener _zRotationAnim;
        private Tweener _yRotationAnim;
        private Tweener _throwTween;
        private CapsuleCollider _collider;
        private Damage _damage;
        private Transform _mainCam;
        private Transform _swordBody;
        private EnemyStateMachine _currentEnemy;
        private AudioSource _audioSource;

        public bool IsInHand => transform.parent == _handHolder;
        public bool IsInSheath => transform.parent == _sheahtHolder;

        private void Awake()
        {
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
                Vector3 dir = Vector3.Slerp(transform.position, _handHolder.position, _curvePointToHolderReturnLerpTime);
                _rb.MovePosition(dir);
                if (Vector3.Distance(_rb.position, _handHolder.position) < 1f)
                {
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
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!_isReturning && _isOnThrow)
            {
                _isOnThrow = false;
                _rb.velocity = Vector3.zero;
                _yRotationAnim.Pause();
                _rb.isKinematic = true;
                transform.SetParent(other.transform);
                if (other.CompareTag("Enemy"))
                {
                    if(other.TryGetComponent(out EnemyStateMachine enemy))
                    {
                        _currentEnemy = enemy;
                        _currentEnemy.ChangeState(_currentEnemy.swordHitState);
                        _currentEnemy.isSwordOn = true;
                        _currentEnemy.sword = this;
                        _isOnEnemy = true;
                        _freezeElectricFX.gameObject.SetActive(true);
                    }

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
            if(_isOnEnemy)
            {
                _currentEnemy.ChangeState(_currentEnemy.chaseState);
                _freezeElectricFX.gameObject.SetActive(false);
                _currentEnemy.isSwordOn = false;
                _isOnEnemy = false;
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
        }
        
        public void StopAttack()
        {
            _collider.enabled = false;
            _damage.enabled = false;
            _trail.enabled = false;
        }

        public void DetachFromEnemy()
        {
            _freezeElectricFX.gameObject.SetActive(false);
            _currentEnemy.isSwordOn = false;
            _isOnEnemy = false;
        }

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
    }

}
