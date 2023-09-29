using DG.Tweening;
using States;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public event System.Action OnRestart;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public void EndGame()
    {

    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        OnRestart?.Invoke();
    }

    private void HandleEndGame()
    {
        Time.timeScale = 0.1f;
    }
    public void QuitGame()
    {
        Debug.Log("Quit");
    }
}
