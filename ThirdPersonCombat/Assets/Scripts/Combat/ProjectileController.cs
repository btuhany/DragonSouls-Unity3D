using Combat;
using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private int _attackDamage;
    [SerializeField] private float _speed = 10f;
    private Damage _damage;
    private Rigidbody _rb;
    public Transform targetTransform;
    private Vector3 _dir;
    [SerializeField] private ParticleSystem postDestroyFX;
    private bool _isStop;
    private void Awake()
    {
        postDestroyFX.Stop();
        _damage = GetComponent<Damage>();
        _rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        _isStop = false;
        _dir = targetTransform.position - _rb.position;
    }
    private void OnDisable()
    {
        _damage.ResetState();
    }
    private void Start()
    {
        _damage.AttackDamage = _attackDamage;
       // _rb.DOMove(targetTransform.position, 2f).SetEase(Ease.InCubic);
    }

    private void FixedUpdate()
    {
        if (_isStop) return;
        _rb.MovePosition(_rb.position + _dir.normalized * Time.fixedDeltaTime * _speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        postDestroyFX.Play();
        _isStop = true;
        Destroy(this.gameObject);   
    }
}
