using DG.Tweening;
using UnityEngine;
using Combat;

namespace PlayerController
{
    public class Sword : MonoBehaviour
    {
        [SerializeField] private float _curvePointToHolderReturnLerpTime = 0.3f;
        [SerializeField] private Transform _curvePoint;
        [SerializeField] private Transform _handHolder;
        [SerializeField] private Transform _sheahtHolder;
        [SerializeField] private float _curvePointReachingSpeed = 10f;
        [SerializeField] private float _targetReachingSpeed = 20f;
        [SerializeField] private int _damageInAir = 25;
        [Header("Sword Throw")]
        [SerializeField] private LayerMask _aimLayer;
        [SerializeField] private float _aimRange;
        private float _curvePointReachingTime = 1.0f;
        private bool _isReturning = false;
        private bool _isOnThrow = false;
        private bool _isInCurvePoint = false;
        private Rigidbody _rb;
        private Tweener _zRotationAnim;
        private Tweener _yRotationAnim;
        private CapsuleCollider _collider;
        private Damage _damage;
        private Transform _mainCam;
        private Transform _swordBody;

        public bool IsEquipped => transform.parent != null;

        private void Awake()
        {
            _swordBody = GetComponentsInChildren<Transform>()[1];
            _damage = GetComponent<Damage>();
            _rb = GetComponent<Rigidbody>();
            _collider = GetComponent<CapsuleCollider>();
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
                _rb.DOMove(hit.point, inAirTime);
            }
            else
            {
                _rb.AddForce(force, ForceMode.Impulse);
            }
        }
        public void Return()
        {
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
        }

        public void Sheath()
        {
            transform.SetParent(_sheahtHolder);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
        }

        public void StartAttack(int damage)
        {
            _collider.enabled = true;
            _damage.ResetState();
            _damage.SetAttackDamage(damage);
            _damage.enabled = true;
        }
        
        public void StopAttack()
        {
            _collider.enabled = false;
            _damage.enabled = false;
        }
    }

}
