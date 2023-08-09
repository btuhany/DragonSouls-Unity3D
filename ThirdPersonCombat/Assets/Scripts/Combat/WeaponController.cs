using Sounds;
using UnityEngine;

namespace Combat
{
    [RequireComponent(typeof(Damage))]
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private SoundClips _weaponHitSounds;
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
        }

        public void StopAttack()
        {
            _damage.ResetState();
            _collider.enabled = false;
            _damage.enabled = false;
        }
        
        public void PlayWeaponHitSFX(Collider other)
        {
            _audioSource.volume = _weaponHitSounds.Volume;
            _audioSource.pitch = _weaponHitSounds.Pitch;
            _audioSource.PlayOneShot(_weaponHitSounds.AudioClips[Random.Range(0, _weaponHitSounds.AudioClips.Length)]);
        }

    }
}

