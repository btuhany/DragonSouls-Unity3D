using Sounds;
using System.Collections;
using UnityEngine;
public class SkeletonSoundController : EnemySoundController
{
    [SerializeField] private SoundClips _bones;
    [SerializeField] private SoundClips _grunts;
    [SerializeField] private SoundClips _sword;
    [SerializeField] private AudioSource _secondarySource;
    [SerializeField] private AudioSource _tertiarySource;
    public override void PlayFootStepSFX()
    {
        base.PlayFootStepSFX();
        PlayBonesSFX();
    }

    public override void PlayAttackSFX()
    {
        _secondarySource.volume = attack.Volume;
        _secondarySource.pitch = Random.Range(attack.Pitch - 0.2f, attack.Pitch + 0.2f);
        _secondarySource.PlayOneShot(attack.AudioClips[Random.Range(0, attack.AudioClips.Length)]);
        PlayBonesSFX();
        audioSource.volume = _bones.Volume - 0.2f;
    }
    private void PlayBonesSFX()
    {
        audioSource.volume = _bones.Volume;
        audioSource.pitch = _bones.Pitch;
        audioSource.PlayOneShot(_bones.AudioClips[Random.Range(0, _bones.AudioClips.Length)]);
    }

    public void PlayGruntsSFX(bool play)
    {
        if(play)
        {
            _secondarySource.volume = _grunts.Volume;
            _secondarySource.pitch = Random.Range(_grunts.Pitch - 0.1f, _grunts.Pitch + 0.1f);
            _secondarySource.PlayOneShot(_grunts.AudioClips[Random.Range(0, _grunts.AudioClips.Length)]);
        }
        else
        {
            _secondarySource.Stop();
        }
    }

    public void PlaySwordSFX(int isGroundHit = 0)
    {
        audioSource.volume = _sword.Volume;
        audioSource.pitch = _sword.Pitch;
        if(isGroundHit == 1)
        {
            audioSource.PlayOneShot(_sword.AudioClips[_sword.AudioClips.Length - 1]);
        }
        audioSource.PlayOneShot(_sword.AudioClips[Random.Range(0, _sword.AudioClips.Length - 1)]);
    }
    public override void PlayGetHitSFX()
    {
        audioSource.Stop();
        _secondarySource.Stop();
        _tertiarySource.volume = getHit.Volume;
        _tertiarySource.pitch = getHit.Pitch;
        _tertiarySource.PlayOneShot(getHit.AudioClips[UnityEngine.Random.Range(0, getHit.AudioClips.Length)]);
        audioSource.volume = 1;
        audioSource.pitch = _bones.Pitch;
        audioSource.PlayOneShot(_bones.AudioClips[Random.Range(0, _bones.AudioClips.Length)]);
    }
}
