using DG.Tweening;
using PlayerController;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private float _returnLerpTime = 0.3f;
    [SerializeField] private float _returnTorqueForce = 30f;
    [SerializeField] private Transform _curvePoint;
    private Transform _swordHolder;
    private Rigidbody _rb;

    private void Awake()
    {
        _swordHolder = transform.parent.transform;
        
        _rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        _rb.velocity = Vector3.zero; 
        _rb.isKinematic = true;
    }
    public void Throwed(Vector3 force, float torqueMagnitude)
    {
        transform.parent = null;
        _rb.isKinematic = false;
        _rb.AddForce(force, ForceMode.Impulse);
        _rb.AddTorque(transform.right * torqueMagnitude, ForceMode.Impulse);
    }
    public void Return()
    {
        _rb.velocity = Vector3.zero;
        _rb.isKinematic = false;
        _rb.AddTorque(transform.right * _returnTorqueForce, ForceMode.Impulse);

        transform.DOMove(_curvePoint.position, 1).SetEase(Ease.InCubic).onComplete = () =>
        {
            transform.DOMove(_swordHolder.position, 0.4f).SetEase(Ease.InFlash).onComplete = () =>
            {
                transform.SetParent(_swordHolder);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.Euler(Vector3.zero);
            };
        };
    }
}
