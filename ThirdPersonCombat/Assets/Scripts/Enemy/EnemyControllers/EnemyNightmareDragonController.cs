using UnityEngine;

public class EnemyNightmareDragonController : MonoBehaviour
{
    [Header("FireProjectile")]
    [SerializeField] private DragonFireProjectile _fireProjectile;
    [SerializeField] private Transform _fireProjectileSpawnPoint;

    [Header("FireWall")]
    [SerializeField] private DragonFireWall _fireWall;
    [SerializeField] private Transform _fireWallSpawnPoint;

    [Header("FireBreath")]
    [SerializeField] private ParticleSystem _fireBreath;
    
    [Header("Fireball")]
    [SerializeField] private Transform _fireballSpawnPoint;
    [SerializeField] private Rigidbody _fireball;
    [SerializeField] private float _maxSpeed = 500f;
    [SerializeField] private float _minSpeed = 300f;
    [SerializeField][Range(0, 1f)] private float _maxUpAxisMag = 1.0f;
    [SerializeField][Range(0, 1f)] private float _maxRightAxisMag = 0.5f;
    [SerializeField][Range(0, 1f)] private float _maxForwardAxisMag = 1.0f;
    [SerializeField][Range(0, 1f)] private float _minForwardAxisMag = 0.5f;

    private Transform _playerTransform => States.PlayerStateMachine.Instance.transform;

    //Animation events
    public void SpawnFireball()
    {
        //Rigidbody fireball = Instantiate(_fireball, _fireballSpawnPoint.position, _fireballSpawnPoint.rotation);
        DragonFireball fireball = BossFireballPool.Instance.GetObject();
        fireball.transform.position = _fireballSpawnPoint.position;
        fireball.transform.rotation = _fireballSpawnPoint.rotation;

        float randomRightAxisVal = Random.Range(-_maxRightAxisMag, _maxRightAxisMag);
        float randomUpAxisVal = Random.Range(0f, _maxUpAxisMag);
        float randomForwardAxisVal = Random.Range(_minForwardAxisMag, _maxForwardAxisMag);
        float randomSpeed = Random.Range(_minSpeed, _maxSpeed);

        fireball.rb.AddForce(_fireballSpawnPoint.TransformDirection(
            Vector3.forward * _maxForwardAxisMag + 
            Vector3.right * randomRightAxisVal + 
            Vector3.up * randomUpAxisVal)
            * randomSpeed);
    }

    public void SetActiveFireBreath(int active)
    {
        if(active == 0)
        {
            _fireBreath.Play();
        }
        else
        {
            _fireBreath.Stop();
        }
    }

    public void ThrowFireProjectile()
    {
        //DragonFireProjectile fireProjectile = Instantiate(_fireProjectile, _fireProjectileSpawnPoint.position, _fireProjectileSpawnPoint.rotation);
        DragonFireProjectile fireProjectile = BossFireProjectilePool.Instance.GetObject();
        fireProjectile.transform.position = _fireProjectileSpawnPoint.position;
        fireProjectile.transform.rotation = _fireProjectileSpawnPoint.rotation;

        Vector3 dir = _playerTransform.position - this._fireballSpawnPoint.position;
        dir.y += 1.3f;
        
        fireProjectile.SetVelocityDirection(dir);
    }

    public void ThrowFireWall()
    {
        //DragonFireWall fireWall = Instantiate(_fireWall, _fireWallSpawnPoint.position, _fireWallSpawnPoint.rotation);
        DragonFireWall fireWall = BossFirewallPool.Instance.GetObject();
        fireWall.transform.position = _fireWallSpawnPoint.position;
        fireWall.transform.rotation = _fireWallSpawnPoint.rotation;

        Vector3 dir = _playerTransform.position - this._fireWallSpawnPoint.position;
        dir.y = 0f;

        fireWall.SetVelocityDirection(dir);
    }
}
