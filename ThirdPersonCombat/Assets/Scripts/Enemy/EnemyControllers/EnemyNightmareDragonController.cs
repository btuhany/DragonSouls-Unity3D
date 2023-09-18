using UnityEngine;

public class EnemyNightmareDragonController : MonoBehaviour
{
    [Header("Fireball")]
    [SerializeField] private Transform _fireballSpawnPoint;
    [SerializeField] private Rigidbody _fireball;
    [SerializeField] private float _maxSpeed = 500f;
    [SerializeField] private float _minSpeed = 300f;
    [SerializeField][Range(0, 1f)] private float _maxUpAxisMag = 1.0f;
    [SerializeField][Range(0, 1f)] private float _maxRightAxisMag = 0.5f;
    [SerializeField][Range(0, 1f)] private float _maxForwardAxisMag = 1.0f;
    [SerializeField][Range(0, 1f)] private float _minForwardAxisMag = 0.5f;
    public void SpawnFireball()
    {
        Rigidbody fireball = Instantiate(_fireball, _fireballSpawnPoint.position, _fireballSpawnPoint.rotation);

        float randomRightAxisVal = Random.Range(-_maxRightAxisMag, _maxRightAxisMag);
        float randomUpAxisVal = Random.Range(0f, _maxUpAxisMag);
        float randomForwardAxisVal = Random.Range(_minForwardAxisMag, _maxForwardAxisMag);
        float randomSpeed = Random.Range(_minSpeed, _maxSpeed);

        fireball.AddForce(_fireballSpawnPoint.TransformDirection(
            Vector3.forward * _maxForwardAxisMag + 
            Vector3.right * randomRightAxisVal + 
            Vector3.up * randomUpAxisVal)
            * randomSpeed);
    }
}
