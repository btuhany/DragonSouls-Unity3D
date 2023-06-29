using DG.Tweening;
using PlayerController;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private float _curvePointToHolderReturnLerpTime = 0.3f;
    [SerializeField] private Transform _curvePoint;
    [SerializeField] private float _curvePointReachingTime = 1.0f;
    private Transform _swordHolder;
    private Rigidbody _rb;
    private bool _isInCurvePoint = false;
    private Tweener _tweener;
    private bool _isReturn = false;
    public bool IsEquipped => transform.parent != null;

    private void Awake()
    {
        _swordHolder = transform.parent.transform;
        _rb = GetComponent<Rigidbody>();
        _tweener = _rb.DORotate(new Vector3(0f, 0f, -180f), 0.1f).SetLoops(-1, LoopType.Incremental);
        _tweener.Pause();
    }
    private void FixedUpdate()
    {
        if(_isInCurvePoint)
        {
            Vector3 dir = Vector3.Slerp(transform.position, _swordHolder.position, _curvePointToHolderReturnLerpTime);
            _rb.MovePosition(dir);
            if(Vector3.Distance(_rb.position, _swordHolder.position) < 1f)
            {
                _tweener.Pause();
                _isInCurvePoint = false;
                _isReturn = false;
                _rb.isKinematic = true;
                transform.SetParent(_swordHolder);
                _rb.velocity = Vector3.zero;
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.Euler(Vector3.zero);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(!_isReturn)
        {
            _rb.velocity = Vector3.zero; 
            _tweener.Pause();
            _rb.isKinematic = true;
        }
    }
    public void Throwed(Vector3 force)
    {
        _rb.isKinematic = false;
        transform.parent = null;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        _tweener.Play();
        _rb.AddForce(force, ForceMode.Impulse);
    }
    public void Return()
    {
        _isReturn = true;
        _rb.isKinematic = false;
        _rb.velocity = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        _tweener.Play();
        _rb.DOMove(_curvePoint.position, _curvePointReachingTime).SetEase(Ease.InCubic).onComplete = () =>
        {
            _isInCurvePoint = true;
        };
    }
}
