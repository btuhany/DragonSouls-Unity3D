using UnityEngine;

namespace Sounds
{
    public abstract class EnemySoundController : MonoBehaviour
    {
        [SerializeField] private SoundClips _footSteps;
        [SerializeField] protected SoundClips attack;
        [SerializeField] private SoundClips _dead;
        [SerializeField] protected SoundClips getHit;

        protected AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public virtual void PlayFootStepSFX()
        {
            audioSource.volume = _footSteps.Volume;
            audioSource.pitch = _footSteps.Pitch;
            audioSource.PlayOneShot(_footSteps.AudioClips[UnityEngine.Random.Range(0, _footSteps.AudioClips.Length)]);
        }
        public abstract void PlayAttackSFX();

        public void PlayDeadSFX()
        {
            audioSource.volume = _dead.Volume;
            audioSource.pitch = _dead.Pitch;
            audioSource.PlayOneShot(_dead.AudioClips[UnityEngine.Random.Range(0, _dead.AudioClips.Length)]);
        }
        public abstract void PlayGetHitSFX();

    }
}
