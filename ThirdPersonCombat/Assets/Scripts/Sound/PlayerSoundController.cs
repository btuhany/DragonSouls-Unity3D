using System;
using UnityEngine;

namespace Sounds
{
    [Serializable]
    public struct SoundClips
    {
        public AudioClip[] AudioClips;
        public float Volume;
        public float Pitch;
    }
    public class PlayerSoundController : MonoBehaviour
    {
        [SerializeField] private SoundClips _runFootSteps;
        [SerializeField] private SoundClips _walkFootSteps;
        [SerializeField] private SoundClips _rollStart;
        [SerializeField] private SoundClips _rollEnd;
        [SerializeField] private SoundClips _hurt;
        [SerializeField] private SoundClips _grunt;

        [Header("AttackFootStep")]
        [SerializeField] private float _attackFootStepSFXVolume = 0.6f;
        [SerializeField] private float _attackFootStepSFXPitch = 1.2f;
        private AudioSource _audioSource;
        private AudioSource _secondarySource;

        private void Awake()
        {
            _audioSource = GetComponents<AudioSource>()[0];
            _secondarySource = GetComponents<AudioSource>()[1];
        }
        public void PlayRandomRunStepSFX()
        {
            _audioSource.volume = _runFootSteps.Volume;
            _audioSource.pitch = _runFootSteps.Pitch;
            _audioSource.PlayOneShot(_runFootSteps.AudioClips[UnityEngine.Random.Range(0, _runFootSteps.AudioClips.Length)]);
        }
        public void PlayRandomAttackFootStepSFX()
        {
            _audioSource.volume = _attackFootStepSFXVolume;
            _audioSource.pitch = _attackFootStepSFXPitch;
            _audioSource.PlayOneShot(_runFootSteps.AudioClips[UnityEngine.Random.Range(0, _runFootSteps.AudioClips.Length)]);
        }
        public void PlayRandomWalkStepSFX()
        {
            _audioSource.volume = _walkFootSteps.Volume;
            _audioSource.pitch = _walkFootSteps.Pitch;
            _audioSource.PlayOneShot(_walkFootSteps.AudioClips[UnityEngine.Random.Range(0, _walkFootSteps.AudioClips.Length)]);
        }
        public void PlayRollSFX(int value)
        {
            if(value == 0)
            {
                _audioSource.volume = _rollStart.Volume;
                _audioSource.pitch = _rollStart.Pitch;
                _audioSource.PlayOneShot(_rollStart.AudioClips[UnityEngine.Random.Range(0, _rollStart.AudioClips.Length)]);
            }
            else
            {
                _audioSource.volume = _rollEnd.Volume;
                _audioSource.pitch = _rollEnd.Pitch;
                _audioSource.PlayOneShot(_rollEnd.AudioClips[UnityEngine.Random.Range(0, _rollEnd.AudioClips.Length)]);
            }
        }
        public void PlayHurtSFX()
        {
            _secondarySource.volume = _hurt.Volume;
            _secondarySource.pitch = _hurt.Pitch;
            _secondarySource.PlayOneShot(_hurt.AudioClips[UnityEngine.Random.Range(0, _hurt.AudioClips.Length)]);
        }
        public void PlayGruntSFX()
        {
            _secondarySource.volume = _grunt.Volume;
            _secondarySource.pitch = _grunt.Pitch + UnityEngine.Random.Range(-0.05f, 0.05f);
            _secondarySource.PlayOneShot(_grunt.AudioClips[UnityEngine.Random.Range(0, _grunt.AudioClips.Length)]);
        }
    }

}
