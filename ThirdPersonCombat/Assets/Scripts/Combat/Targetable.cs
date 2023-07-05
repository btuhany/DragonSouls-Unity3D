using UnityEngine;

public class Targetable : MonoBehaviour
{
    [SerializeField] private TargetableCanvasHandler _targetableCanvasHandler;
    public event System.Action<Targetable> OnDestroyedDisabled;
    private void Awake()
    {
        if(!_targetableCanvasHandler)
            _targetableCanvasHandler = GetComponentInChildren<TargetableCanvasHandler>();
    }
    private void OnDestroy()
    {
        OnDestroyedDisabled?.Invoke(this);
    }
    private void OnDisable()
    {
        OnDestroyedDisabled?.Invoke(this);
    }
    public void SetTargetedState(bool isTargeted)
    {
        _targetableCanvasHandler.SetActive(isTargeted);
    }
}
