using DG.Tweening;
using TMPro;
using UnityEngine;

public class SoulsManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _soulText;
    [SerializeField] private float _particleShortDistFactor = 5f;
    [SerializeField] private float _particleMediumFactor = 5f;
    [SerializeField] private float _particleLongFactor = 5f;
    [SerializeField] private Ease _particleMoveAnimEase = Ease.Linear;
    [SerializeField] private ParticleSystem _particleFX;
    private int _currentSouls;
    public static SoulsManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }
    private void OnEnable()
    {
        _currentSouls = 0;
        _soulText.text = _currentSouls.ToString();
    }
    public void AddSoul(int value, Vector3 pos)
    {
        Vector3 target = States.PlayerStateMachine.Instance.transform.position + Vector3.up * 1.7f;
        ParticleSystem particleFX = Instantiate(_particleFX, pos + Vector3.up * 2f, Quaternion.identity);
        particleFX.gameObject.transform.rotation = Quaternion.LookRotation(
            Vector3.up, particleFX.transform.InverseTransformDirection(target));
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
            _currentSouls += value;
            _soulText.text = _currentSouls.ToString();
        };
        particleFX.Play();
    }
}
