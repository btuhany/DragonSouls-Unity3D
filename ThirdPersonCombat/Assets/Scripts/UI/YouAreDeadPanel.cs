using DG.Tweening;
using States;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class YouAreDeadPanel : MonoBehaviour
{
    public Image deadPanel;
    public TextMeshProUGUI deadText;
    public Image blackScreen;

    private void Start()
    {
        PlayerStateMachine.Instance.health.OnDead += HandleOnPlayerDeath;
        PlayerStateMachine.Instance.OnPlayerRespawn += HandleOnPlayerRespawn;
    }
    public void HandleOnPlayerDeath()
    {
        deadPanel.gameObject.SetActive(true);
        deadText.gameObject.SetActive(true);
        blackScreen.gameObject.SetActive(true);
        deadPanel.DOFade(1f, 1.5f);
        deadText.DOFade(1f, 1.5f);
        blackScreen.DOFade(1f, 4f);
    }
    public void HandleOnPlayerRespawn()
    {
        deadPanel.DOFade(0f, 0.5f);
        deadText.DOFade(0f, 0.5f);
        blackScreen.DOFade(0f, 0.5f).onComplete = () =>
        {
            deadPanel.gameObject.SetActive(false);
            deadText.gameObject.SetActive(false);
            blackScreen.gameObject.SetActive(false);
        };
    }
}
