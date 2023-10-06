using DG.Tweening;
using States;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public event System.Action OnRestart;
    public LoadingPanel loadingPanel;
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
    public void StartGame()
    {
        SoundManager.Instance.PlayAuidoClip(4, 0);
        StartCoroutine(LoadNextSceneFromIndexAsync(1));
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
    private IEnumerator LoadNextSceneFromIndexAsync(int sceneIndex, bool stopTime = false)
    {
        loadingPanel?.StartLoading();
        if (stopTime)
            Time.timeScale = 0f;
        loadingPanel?.gameObject.SetActive(true);
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + sceneIndex);
        loadingPanel?.UpdateLoadingBar(loadOperation.progress);
        while (!loadOperation.isDone)
        {
            loadingPanel?.UpdateLoadingBar(loadOperation.progress);
            yield return null;
        }
        Time.timeScale = 1f;
    }
    private IEnumerator LoadSceneFromIndexAsync(int sceneIndex, bool stopTime = false)
    {
        loadingPanel?.StartLoading();
        if (stopTime)
            Time.timeScale = 0f;
        loadingPanel?.gameObject.SetActive(true);
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!loadOperation.isDone)
        {
            loadingPanel?.UpdateLoadingBar(loadOperation.progress);
            yield return null;
        }
        Time.timeScale = 1f;
    }
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    public void QuitToMainMenu()
    {
        StartCoroutine(LoadSceneFromIndexAsync(0, true));
    }
}
