using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class SoulsManager : MonoBehaviour
{
    [SerializeField] private int _initialSouls = 0;
    [SerializeField] private TextMeshProUGUI _soulText;
    [SerializeField] private TextMeshProUGUI _addedSoulsText;
    [SerializeField] private float _particleShortDistFactor = 5f;
    [SerializeField] private float _particleMediumFactor = 5f;
    [SerializeField] private float _particleLongFactor = 5f;
    [SerializeField] private Ease _particleMoveAnimEase = Ease.Linear;
    [SerializeField] private ParticleSystem _particleFX;
    private int _currentSouls;
    public static SoulsManager Instance;
    public int CurrentSouls { get => _currentSouls;  }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void OnEnable()
    {
        _currentSouls = _initialSouls;
        _soulText.text = _currentSouls.ToString();
    }
    public bool SpendSoul(int value)
    {
        if (value <= _currentSouls)
        {
            _currentSouls -= value;
            _soulText.text = _currentSouls.ToString();
            return true;
        }
        else
        {
            return false;
        }

    }
    public void AddSoul(int value, Vector3 pos)
    {
        //Handle souls gather particleFX
        Vector3 target = States.PlayerStateMachine.Instance.transform.position + Vector3.up * 1.7f;
        //ParticleSystem particleFX = Instantiate(_particleFX, pos + Vector3.up * 2f, Quaternion.identity);
        SoulParticles particleObj = SoulParticlesPool.Instance.GetObjectDisabled();
        particleObj.transform.position = pos + Vector3.up * 2f;
        particleObj.transform.rotation = Quaternion.identity;
        ParticleSystem particleFX = particleObj.particleFx;
        particleFX.gameObject.transform.rotation = Quaternion.LookRotation(
            Vector3.up, target - particleFX.transform.position);
        float distance = Vector3.Distance(target, pos);
        float animTime;
        if (distance < 1f)
        {
            animTime = _particleShortDistFactor / distance;
        }
        else if(distance < 8f)
        {
            animTime = _particleMediumFactor / distance;
        }
        else
        {
            animTime = _particleLongFactor / distance;
        }
        animTime = Mathf.Max(animTime, 1.5f);
        particleFX.gameObject.transform.DOMove(States.PlayerStateMachine.Instance.transform.position + Vector3.up * 1.2f, animTime).SetEase(_particleMoveAnimEase).onComplete = () =>
        {
            SoundManager.Instance.PlayAuidoClip(2, 0);
            SoulParticlesPool.Instance.ReturnObject(particleObj);
            StartCoroutine(AddSoulsAnim(value));
        };
        particleObj.gameObject.SetActive(true);
        particleFX.Play();
    }

    WaitForSeconds _soulAddDelay = new WaitForSeconds(0.1f);


    private IEnumerator AddSoulsAnim(int value)
    {
        _soulAddDelay = new WaitForSeconds(1.5f / value);
        _addedSoulsText.text = $"+{value}";
        _addedSoulsText.DOFade(1, 1f);
        for (int i = 0; i < value; i++)
        {
            _currentSouls++;
            _soulText.text = _currentSouls.ToString();
            yield return _soulAddDelay;
        }
        _addedSoulsText.DOFade(0, 1f);
        yield return null;
    }
    //private IEnumerator AddedSoulsAnim(int value)
    //{
    //    _addedSoulsText.text = $"+{value}";
    //    _addedSoulsText.DOFade(1, 1f);
        
    //    _addedSoulsText.DOFade(0, 1f);
    //}
    public void ResetSouls()
    {
        _currentSouls = 0;
        _soulText.text = _currentSouls.ToString();
    }
}
