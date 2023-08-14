using Combat;
using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private int _attackDamage;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private bool _chasePlayerInitial = false;
    [SerializeField] private bool _useTween = false;
    [SerializeField] private Ease _tweenEase = Ease.Linear;
    private Damage _damage;
    private Rigidbody _rb;
    public Transform targetTransform;
    private Vector3 _dir;
    [SerializeField] private ParticleSystem postDestroyFX;
    private bool _chasePlayer = false;
    private bool _isStop;
    private Tweener _movetargetAnim;
    private void Awake()
    {
        postDestroyFX.Stop();
        _damage = GetComponent<Damage>();
        _rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        _chasePlayer = _chasePlayerInitial;
        _damage.OnHitGiven += HandleOnDamageGiven;

        if(_useTween)
        {
            _isStop = true;
            Vector2 randomVec = Random.onUnitSphere * 1.3f;
            transform.position += transform.right * randomVec.x + transform.up * randomVec.y;
            _movetargetAnim = _rb.DOMove(targetTransform.position, 2f).SetEase(_tweenEase);
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
        Destroy(this.gameObject);
    }
}
