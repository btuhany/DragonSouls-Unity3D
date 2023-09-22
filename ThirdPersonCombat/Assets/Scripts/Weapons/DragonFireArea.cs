using Combat;
using UnityEngine;

public class DragonFireArea : MonoBehaviour
{
    [SerializeField] private int _damagePoint = 7;
    [SerializeField] private float _damageTimePeriod = 0.6f;
    [SerializeField] private float _maxLifeTime = 4.7f;
    private Damage _damage;
    private BoxCollider _collider;
    private Health _targetHealth => States.PlayerStateMachine.Instance.health;
    private float _damageTimeCounter;
    private float _lifeTimeCounter;
    //private AudioSource _audioSource;

    private void Awake()
    {
        _damage = GetComponent<Damage>();
        _collider = GetComponent<BoxCollider>();
        //_audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        _lifeTimeCounter = 0f;
        _damageTimeCounter = 0f;
        StartAttack(_damagePoint);
    }
    private void OnDisable()
    {
        _damage.ResetState();
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (_targetHealth != null) return;
    //    if(other.CompareTag("Player"))
    //    {
    //        if(TryGetComponent(out Health health))
    //        {
    //            _targetHealth = health;
    //        }
    //    }
    //}
    private void Update()
    {
        _lifeTimeCounter += Time.deltaTime;
        if(_lifeTimeCounter > _maxLifeTime )
        {
            _lifeTimeCounter = 0f;
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        _damageTimeCounter += Time.deltaTime;
        if(_damageTimeCounter >= _damageTimePeriod)
        {
            _damageTimeCounter = 0f;
            _damage.GiveDamageForced(_targetHealth, other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        _damage.ResetState();
        _damageTimeCounter = 0f;
    }
    private void StartAttack(int damage)
    {
        _collider.enabled = true;
        _damage.ResetState();
        _damage.SetAttackDamage(damage);
        _damage.enabled = true;
    }
}
