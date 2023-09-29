using DG.Tweening;
using States;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
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
        Time.timeScale = 0;
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
    }

}
