using DG.Tweening;
using UnityEngine;
using static UnityEngine.InputManagerEntry;

public class BossManager : MonoBehaviour
{
    private bool _isBossStopped;
    [Header("Boss")]
    [SerializeField] private GameObject _bossHealthUI;
    [SerializeField] private GameObject _bossWall;
    [SerializeField] private AiAgent _boss;

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
    }
    private void Start()
    {
        _boss.health.OnDead += HandleOnBossDeath;
    }
    public void StartBossFight()
    {
        if (_isBossStopped)
            _boss.StartAgent();
        _isBossStopped = false;
        IsInBoss = true;
        _bossHealthUI.SetActive(true);
        _bossWall.transform.DOMove(_bossWall.transform.position + _bossWall.transform.TransformDirection(Vector3.forward * 17f), 0.6f).SetEase(Ease.Linear);
    }
    public void ExitBossFight()
    {
        _boss.health.ResetHealth();
        IsInBoss = false;
        _bossHealthUI.SetActive(false);
        _bossWall.transform.DOMove(_bossWall.transform.position + _bossWall.transform.TransformDirection(Vector3.back * 17f), 0.6f).SetEase(Ease.Linear);
        _boss.StopAgent();
        _isBossStopped = true;
    }
    public void HandleOnBossDeath()
    {
        _bossHealthUI.SetActive(false);
        OnBossDefeated?.Invoke();
        GameManager.Instance.EndGame();
    }
}
