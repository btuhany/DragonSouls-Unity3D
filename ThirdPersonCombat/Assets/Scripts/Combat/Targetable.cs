using UnityEngine;

public class Targetable : MonoBehaviour
{
    public Transform TargetPoint;
    public event System.Action<Targetable> OnDestroyedDisabled;

    private void OnDestroy()
    {
        OnDestroyedDisabled?.Invoke(this);
    }
    private void OnDisable()
    {
        OnDestroyedDisabled?.Invoke(this);
    }
    public void ResetTargetable()
    {
        OnDestroyedDisabled?.Invoke(this);
    }
}
