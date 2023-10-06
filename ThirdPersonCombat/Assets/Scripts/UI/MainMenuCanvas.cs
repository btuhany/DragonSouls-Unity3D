using UnityEngine;
using UnityEngine.UI;

public class MainMenuCanvas : MonoBehaviour
{
    [SerializeField] Button _firstSelectedButton;
    [SerializeField] GameObject _panel;
    private void OnEnable()
    {
        _firstSelectedButton.Select();
    }
    public void StartGameButton()
    {
        SoundManager.Instance.PlayAuidoClip(0, 0);
        GameManager.Instance.StartGame();
        _panel.SetActive(false);
    }
    public void QuitGameButton()
    {
        SoundManager.Instance.PlayAuidoClip(0, 0);
        GameManager.Instance.QuitGame();
    }
}
