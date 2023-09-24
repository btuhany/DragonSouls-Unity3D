using UnityEngine;
using UnityEngine.InputSystem;
using Combat;

namespace Inputs
{
    public class InputReader : MonoBehaviour, Controllers.IPlayerActions
    {
        public Vector2 MovementOn2DAxis { get; private set; }
        public Vector2 CameraMovementOn2DAxis { get; private set; }
        public bool SprintHold { get; private set; }
        public bool AimHold { get; private set; }
        public event System.Action JumpEvent;
        public event System.Action DodgeEvent;
        public event System.Action TargetEvent;
        public event System.Action SprintEvent;
        public event System.Action SprintHoldEvent;
        public event System.Action SprintHoldCanceledEvent;
        public event System.Action LightAttackEvent;
        public event System.Action HeavyAttackEvent;
        public event System.Action SheathUnsheathSword;
        public event System.Action AimHoldEvent;
        public event System.Action AimHoldCancelEvent;
        public event System.Action WeaponReturnEvent;
        public event System.Action RollEvent;
        public event System.Action<Vector2> TargetSelectEvent;
        public event System.Action HealEvent;
        public AttackType LastAttackType { get; private set; }

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
            CameraMovementOn2DAxis = context.ReadValue<Vector2>();
        }

        public void OnTarget(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }
            TargetEvent?.Invoke();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }
            SprintEvent?.Invoke();
        }

        public void OnHoldSprint(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                SprintHoldEvent?.Invoke();
                SprintHold = true;
            }
            if (context.canceled)
            {
                SprintHoldCanceledEvent?.Invoke();
                SprintHold = false;
            }
        }

        public void OnLightAttack(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }
            LastAttackType = AttackType.Light;
            LightAttackEvent?.Invoke();
        }

        public void OnHeavyAttack(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }
            LastAttackType = AttackType.Heavy;
            HeavyAttackEvent?.Invoke();
        }

        public void OnSheathSword(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }
            SheathUnsheathSword?.Invoke();
        }

        public void OnAim(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                AimHoldEvent?.Invoke();
                AimHold = true;
            }
            if (context.canceled)
            {
                AimHoldCancelEvent?.Invoke();
                AimHold = false;
            }
        }

        public void OnWeaponReturn(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }
            WeaponReturnEvent?.Invoke();
        }

        public void OnRoll(InputAction.CallbackContext context)
        {
            if (context.performed && !SprintHold)
            {
                RollEvent?.Invoke();
            }
            //if (context.started)
            //{
            //    Debug.Log("started");
            //}
            //if (context.performed)
            //{
            //    Debug.Log("performed");
            //}
            //if (context.canceled)
            //{
            //    Debug.Log("canceled");
            //}
        }

        public void OnTargetSelect(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                TargetSelectEvent?.Invoke(context.ReadValue<Vector2>());
            }
        }

        public void OnHeal(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                HealEvent?.Invoke();
            }
        }
    }
}
