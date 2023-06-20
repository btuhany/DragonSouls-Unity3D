using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controllers.IPlayerActions
{
    public event System.Action JumpEvent;
    public event System.Action DodgeEvent;

    private Controllers controls;
    private void Awake()
    {
        controls = new Controllers();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();
    }
    private void OnDestroy()
    {
        controls.Player.Disable();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if(!context.performed) { return;}
        JumpEvent?.Invoke();
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (!context.performed) { return;}
        DodgeEvent?.Invoke();
    }
}
