using UnityEngine;
using UnityEngine.UI;

public class MainMenuCanvas : MonoBehaviour
{
    [SerializeField] Button _firstSelectedButton;
    private void OnEnable()
    {
        _firstSelectedButton.Select();
    }
    public void StartGameButton()
    {
        Debug.Log("StartTheGame!");
    }
    public void QuitGameButton()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
