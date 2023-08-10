using Sounds;
using UnityEngine;

namespace Combat
{
    [RequireComponent(typeof(Damage))]
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private SoundClips _weaponHitSounds;
        [SerializeField] private SoundClips _weaponSwingSounds;
        private Damage _damage;
        private CapsuleCollider _collider;
        private AudioSource _audioSource;

        private void Awake()
        {
            _damage = GetComponent<Damage>();
            _collider = GetComponent<CapsuleCollider>();
            _audioSource = GetComponent<AudioSource>();
        }
        private void OnEnable()
        {
            _damage.OnHitGiven += PlayWeaponHitSFX;
        }

        public void StartAttack(int damage)
        {
            _collider.enabled = true;
            _damage.ResetState();
            _damage.SetAttackDamage(damage);
            _damage.enabled = true;
            PlayWeaponSwingSFX();
        }

        public void StopAttack()
        {
            _damage.ResetState();
            _collider.enabled = false;
            _damage.enabled = false;
        }
        
        public void PlayWeaponHitSFX(Collider other)
        {
            if (_weaponHitSounds.AudioClips.Length == 0) return;
            _audioSource.volume = _weaponHitSounds.Volume;
            _audioSource.pitch = _weaponHitSounds.Pitch;
            _audioSource.PlayOneShot(_weaponHitSounds.AudioClips[Random.Range(0, _weaponHitSounds.AudioClips.Length)]);
        }

        public void PlayWeaponSwingSFX()
        {
            if (_weaponSwingSounds.AudioClips.Length == 0) return;
            _audioSource.volume = _weaponSwingSounds.Volume;
            _audioSource.pitch = _weaponSwingSounds.Pitch;
            _audioSource.PlayOneShot(_weaponSwingSounds.AudioClips[Random.Range(0, _weaponSwingSounds.AudioClips.Length)]);
        }

    }
}

