using DG.Tweening;
using States;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
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
        PlayerStateMachine.Instance.OnPlayerTeleported += HandleOnPlayerTeleported;
    }

    private void HandleOnPlayerTeleported()
    {
        blackScreen.gameObject.SetActive(true);
        blackScreen.color = Color.black;
        StartCoroutine(BlackScreenWait());
    }
    private WaitForSeconds _blackScreenWait = new WaitForSeconds(3f);
    private IEnumerator BlackScreenWait()
    {
        yield return _blackScreenWait;
        blackScreen.DOFade(0f, 2f).onComplete = () =>
        {
            blackScreen.gameObject.SetActive(false);
        };
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
