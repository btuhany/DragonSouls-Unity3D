using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour
{
    [SerializeField] private Slider _loadingSlider;
    private void Awake()
    {
        _loadingSlider = GetComponentInChildren<Slider>();
    }
    private void Start()
    {
        GameManager.Instance.loadingPanel = this;
        gameObject.SetActive(false);
    }
    public void StartLoading()
    {
        gameObject.SetActive(true);
    }
    public void UpdateLoadingBar(float progress)
    {
        float progressVal = progress / 0.9f;
        _loadingSlider.value = progressVal;
    }
}
