using Combat;
using UnityEngine;

public class DragonFireball : MonoBehaviour
{
    //[SerializeField] private SoundClips _weaponHitSounds;
    //[SerializeField] private SoundClips _weaponSwingSounds;
    [SerializeField] private int _damagePoint = 13;
    [SerializeField] private GameObject _explosionFX;
    private Damage _damage;
    private SphereCollider _collider;
    private AudioSource _audioSource;

    private void Awake()
    {
        _damage = GetComponent<Damage>();
        _collider = GetComponent<SphereCollider>();
        //_audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        //_damage.OnHitGiven += PlayWeaponHitSFX;
        StartAttack(_damagePoint);
    }
    private void OnTriggerEnter(Collider other)
    {
        SpawnLava();
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject, 0.1f);
        }
        else
        {
        }
        
    }
    private void SpawnLava()
    {
        Instantiate(_explosionFX, transform.position, Quaternion.identity).SetActive(true);
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
