using DG.Tweening;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    private bool _isBossStopped;
    [Header("Boss")]
    [SerializeField] private GameObject _bossHealthUI;
    [SerializeField] private GameObject _bossWall;
    [SerializeField] private AiAgent _boss;
    [SerializeField] private ParticleSystem _fireFX;
    private AudioSource _audio;
    public bool IsInBoss;
    public static BossManager Instance;
    public event System.Action OnBossDefeated;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        _audio = GetComponent<AudioSource>();
    }
    private void Start()
    {
        _boss.health.OnDead += HandleOnBossDeath;
        _fireFX.Stop();
    }
    public void StartBossFight()
    {
        if (_isBossStopped)
            _boss.StartAgent();
        _audio.Play();
        _isBossStopped = false;
        IsInBoss = true;
        _bossHealthUI.SetActive(true);
        _bossWall.transform.DOMove(_bossWall.transform.position + _bossWall.transform.TransformDirection(Vector3.forward * 17f), 0.6f).SetEase(Ease.Linear);
        _fireFX.Play();
    }
    public void ExitBossFight()
    {
        _boss.health.ResetHealth();
        IsInBoss = false;
        _bossHealthUI.SetActive(false);
        _bossWall.transform.DOMove(_bossWall.transform.position + _bossWall.transform.TransformDirection(Vector3.back * 17f), 0.6f).SetEase(Ease.Linear);
        _boss.StopAgent();
        _isBossStopped = true;
        _audio.Stop();
        _fireFX.Stop();
    }
    public void HandleOnBossDeath()
    {
        _audio.Stop();
        _bossHealthUI.SetActive(false);
        OnBossDefeated?.Invoke();
        //GameManager.Instance.EndGame();
    }
}
