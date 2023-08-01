using System;
using UnityEngine;

namespace Sounds
{
    public class PlayerSoundController : MonoBehaviour
    {
        [Serializable]
        public struct SoundClips
        {
            public AudioClip[] AudioClips;
            public float Volume;
            public float Pitch;
        }

        [SerializeField] private SoundClips _runFootSteps;
        [SerializeField] private SoundClips _walkFootSteps;
        [SerializeField] private SoundClips _rollStart;
        [SerializeField] private SoundClips _rollEnd;
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }
        public void PlayRandomRunStepSFX()
        {
            _audioSource.volume = _runFootSteps.Volume;
            _audioSource.pitch = _runFootSteps.Pitch;
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
    }

}
