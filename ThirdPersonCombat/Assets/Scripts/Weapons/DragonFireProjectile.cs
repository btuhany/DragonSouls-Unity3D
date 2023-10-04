using Combat;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class DragonFireProjectile : MonoBehaviour
{
    [SerializeField] private float _maxLifeTime = 5.6f;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private int _damagePoint = 10;
    [SerializeField] private ParticleSystem[] _particles;
    private float _lifeTimeCounter;
    private Rigidbody _rigidbody;
    private Damage _damage;
    private SphereCollider _collider;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _damage = GetComponent<Damage>();
        _collider = GetComponent<SphereCollider>();
    }
    private void OnEnable()
    {
        ResetParticles();
        StartAttack(_damagePoint);
        _lifeTimeCounter = 0f;
        _damage.ResetState();
    }
    private void Update()
    {
        _lifeTimeCounter += Time.deltaTime;
        if(_lifeTimeCounter > _maxLifeTime)
        {
            _lifeTimeCounter = 0f;
            BossFireProjectilePool.Instance.ReturnObject(this);
        }
    }
    private void OnDisable()
    {
        _damage.ResetState();
    }
    private void OnTriggerEnter(Collider other)
    {
        ExplosionFX();
    }
    private void StartAttack(int damage)
    {
        _collider.enabled = true;
        _damage.ResetState();
        _damage.SetAttackDamage(damage);
        _damage.enabled = true;
    }
    public void SetVelocityDirection(Vector3 dir)
    {
        _rigidbody.velocity = dir.normalized * _speed;
    }
    private void ResetParticles()
    {
        if (!_particles[0].isPlaying)
            _particles[0].Play();
        if (_particles[1].isPlaying)
            _particles[1].Stop();
    }
    private void ExplosionFX()
    {
        if (_particles[0].isPlaying)
            _particles[0].Stop();
        if (!_particles[1].isPlaying)
            _particles[1].Play();
    }
}
