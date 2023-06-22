using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs
{
    public class InputReader : MonoBehaviour, Controllers.IPlayerActions
    {
        public Vector2 MovementOn2DAxis { get; private set; }
        public event System.Action JumpEvent;
        public event System.Action DodgeEvent;
        public event System.Action TargetEvent;

        private Controllers _controls;
        private void Awake()
        {
            _controls = new Controllers();
            _controls.Player.SetCallbacks(this);
            _controls.Player.Enable();
        }
        private void OnDestroy()
        {
            _controls.Player.Disable();
        }
        public void OnJump(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }
            JumpEvent?.Invoke();
        }

        public void OnDodge(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }
            DodgeEvent?.Invoke();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MovementOn2DAxis = context.ReadValue<Vector2>();
        }

        public void OnCamera(InputAction.CallbackContext context)
        {

        }

        public void OnTarget(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }
            TargetEvent?.Invoke();
        }
    }
}
