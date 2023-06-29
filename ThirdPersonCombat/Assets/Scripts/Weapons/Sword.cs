using DG.Tweening;
using UnityEngine;

namespace PlayerController
{
    public class Sword : MonoBehaviour
    {
        [SerializeField] private float _curvePointToHolderReturnLerpTime = 0.3f;
        [SerializeField] private Transform _curvePoint;
        [SerializeField] private Transform _handHolder;
        [SerializeField] private Transform _sheahtHolder;
        [SerializeField] private float _curvePointReachingSpeed = 10f;
        private float _curvePointReachingTime = 1.0f;
        private Rigidbody _rb;
        private bool _isInCurvePoint = false;
        private Tweener _tweener;
        private bool _isReturning = false;
        private bool _isOnThrow = false;
        private CapsuleCollider _collider;
        private Damage _damage;
        public bool IsEquipped => transform.parent != null;

        private void Awake()
        {
            _damage = GetComponent<Damage>();
            _rb = GetComponent<Rigidbody>();
            _collider = GetComponent<CapsuleCollider>();
            _tweener = _rb.DORotate(new Vector3(0f, 0f, -180f), 0.1f).SetLoops(-1, LoopType.Incremental);
            _tweener.Pause();
            _damage.enabled = false;
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
                    _tweener.Pause();
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
                _tweener.Pause();
                _rb.isKinematic = true;
            }
        }
        private void CalculateReturnTime()
        {
            float distance = Vector3.Distance(_curvePoint.position, _rb.transform.position);
            _curvePointReachingTime = distance / _curvePointReachingSpeed;
        }
        public void Throwed(Vector3 force)
        {
            _isOnThrow = true;
            _collider.enabled = true;
            _rb.isKinematic = false;
            transform.parent = null;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            _tweener.Play();
            _rb.AddForce(force, ForceMode.Impulse);
        }
        public void Return()
        {
            CalculateReturnTime();
            _isReturning = true;
            _rb.isKinematic = false;
            _rb.velocity = Vector3.zero;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            _tweener.Play();
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
            _rb.isKinematic = true;
            _damage.ResetState();
            _damage.SetAttackDamage(damage);
            _damage.enabled = true;
        }
        
        public void StopAttack()
        {
            _collider.enabled = false;
            _rb.isKinematic = false;
            _damage.enabled = false;
        }
    }

}
