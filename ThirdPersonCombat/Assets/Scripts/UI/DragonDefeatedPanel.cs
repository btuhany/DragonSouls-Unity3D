using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DragonDefeatedPanel : MonoBehaviour
{
    [SerializeField] private Image _dragonDefeatedPanel;
    [SerializeField] private TextMeshProUGUI _dragonText;
    [SerializeField] private Image _blackPanel;
    [SerializeField] private GameObject _pauseScreen;
    private void Start()
    {
        BossManager.Instance.OnBossDefeated += HandleOnBossDefeated;
        GameManager.Instance.OnRestart += HandleOnRestart;
    }
    public void HandleOnBossDefeated()
    {
        _dragonDefeatedPanel.gameObject.SetActive(true);
        _dragonText.gameObject.SetActive(true);
        _blackPanel.gameObject.SetActive(true);
        _dragonDefeatedPanel.DOFade(1f, 1.5f);
        _dragonText.DOFade(1f, 1f).SetEase(Ease.InBounce);
        _blackPanel.DOFade(1f, 4f);
        StartCoroutine(EndGameDelay());
    }
    public void HandleOnRestart()
    {
        _dragonDefeatedPanel.DOFade(0f, 0.5f);
        _dragonText.DOFade(0f, 0.5f);
        _blackPanel.DOFade(0f, 1f);
        _dragonDefeatedPanel.gameObject.SetActive(false);
        _dragonText.gameObject.SetActive(false);
        _blackPanel.gameObject.SetActive(false);
    }

    WaitForSeconds _delay = new WaitForSeconds(4.5f);
    IEnumerator EndGameDelay()
    {
        yield return _delay;
        _dragonText.DOFade(0f, 1f);
        _pauseScreen.SetActive(true);
        GameManager.Instance.EndGame();
        yield return null;
    }
}
