using Combat;
using UnityEngine;

public class DragonFireball : MonoBehaviour
{
    //[SerializeField] private SoundClips _weaponHitSounds;
    //[SerializeField] private SoundClips _weaponSwingSounds;
    [SerializeField] private int _damagePoint = 13;
    [SerializeField] private GameObject _explosionFX;
    [SerializeField] private GameObject _fireArea;
    private Damage _damage;
    private SphereCollider _collider;
    private AudioSource _audioSource;
    public Rigidbody rb;

    private void Awake()
    {
        _damage = GetComponent<Damage>();
        _collider = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
        //_audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        //_damage.OnHitGiven += PlayWeaponHitSFX;
        StartAttack(_damagePoint);
        rb.velocity = Vector3.zero;
    }
    private void OnTriggerEnter(Collider other)
    {
        SpawnExplosionFX();
        if (!other.gameObject.CompareTag("Player"))
        {
            SpawnFireArea();
        }
        StopAttack();
        BossFireballPool.Instance.ReturnObject(this);
        
    }
    private void SpawnExplosionFX()
    {
        //Instantiate(_explosionFX, transform.position, Quaternion.identity).SetActive(true);
        ExplosionSmoke explosionFX = ExplosionSmokePool.Instance.GetObject();
        explosionFX.transform.position = transform.position;
    }
    private void SpawnFireArea()
    {
        //Instantiate(_fireArea, transform.position, Quaternion.identity).SetActive(true);
        DragonFireArea fireArea = BossFireAreaPool.Instance.GetObject();
        fireArea.transform.position = transform.position;
    }
    private void StartAttack(int damage)
    {
        _collider.enabled = true;
        _damage.ResetState();
        _damage.SetAttackDamage(damage);
        _damage.enabled = true;
        //PlayWeaponSwingSFX();
    }

    private void StopAttack()
    {
        _damage.ResetState();
        _collider.enabled = false;
        _damage.enabled = false;
    }

    //public void PlayWeaponHitSFX(Collider other)
    //{
    //    if (_weaponHitSounds.AudioClips.Length == 0) return;
    //    _audioSource.volume = _weaponHitSounds.Volume;
    //    _audioSource.pitch = _weaponHitSounds.Pitch;
    //    _audioSource.PlayOneShot(_weaponHitSounds.AudioClips[Random.Range(0, _weaponHitSounds.AudioClips.Length)]);
    //}

    //public void PlayWeaponSwingSFX()
    //{
    //    if (_weaponSwingSounds.AudioClips.Length == 0) return;
    //    _audioSource.volume = _weaponSwingSounds.Volume;
    //    _audioSource.pitch = _weaponSwingSounds.Pitch;
    //    _audioSource.PlayOneShot(_weaponSwingSounds.AudioClips[Random.Range(0, _weaponSwingSounds.AudioClips.Length)]);
    //}
}
