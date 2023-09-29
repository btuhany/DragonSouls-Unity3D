using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] private Button _firstSelectedButton;
    private void OnEnable()
    {
        _firstSelectedButton.Select();
    }
    public void RestartButton()
    {
        GameManager.Instance.RestartGame();
    }
    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
}
