using Inputs;
using PlayerController;
using UnityEngine;
using Movement;
using Combat;
using DG.Tweening;

namespace States
{
    public abstract class PlayerBaseState : State
    {
        public bool IsSprintHold = false;
        public bool IsSprint = false;
        protected Transform transform;
        protected Transform mainCamTransform;
        protected InputReader inputReader;
        protected PlayerAnimationController animationController;
        protected PlayerStateMachine stateMachine;
        protected TargetableCheck targetableCheck;
        protected MovementController movement;
        protected ForceReceiver forceReciver;

        public PlayerBaseState(PlayerStateMachine player)
        {
            mainCamTransform = Camera.main.transform;
            this.stateMachine = player;
            this.transform = player.transform;
            this.inputReader = player.InputReader;
            this.animationController = player.animationController;
            this.targetableCheck = player.targetableCheck;
            this.movement = player.movement;
            this.forceReciver = player.forceReceiver;
        }
        public override void Enter()
        {
            IsSprintHold = stateMachine.isSprintHolding;
            IsSprint = stateMachine.isSprinting;
            AddMethodsToEvents();
        }
        public override void Exit()
        {
            stateMachine.isSprintHolding = IsSprintHold;
            stateMachine.isSprinting = IsSprint;
            RemoveMethodsFromEvents();
        }
        protected void MoveCharacter(Vector3 motion, float speed, float deltaTime)
        {
            movement.Move(motion, speed, deltaTime);
        }
        protected void RotateCharacter(Vector3 motionXZAxis, float deltaTime)
        {
            //normalize motionXZAxis 
            movement.LookRotation(motionXZAxis, deltaTime);
        }
        protected void RotateCharacterAttack(Vector3 motionXZAxis, float time)
        {
            if (motionXZAxis != Vector3.zero)
            {
                transform.DOLocalRotateQuaternion(Quaternion.LookRotation(motionXZAxis), time);
            }
        }
        protected void RotateAround(Vector3 rotationVector, float value)
        {
            movement.LookRotationAround(rotationVector, value);
        }
        protected void AddMethodsToEvents()
        {
            inputReader.TargetEvent += HandleOnTargetEvent;
            inputReader.SprintHoldEvent += HandleOnSprintHoldEvent;
            inputReader.SprintHoldCanceledEvent += HandleOnSprintHoldCancelEvent;
            inputReader.SprintEvent += HandleOnSprintEvent;
            inputReader.LightAttackEvent += HandleOnLightAttackEvent;
            inputReader.HeavyAttackEvent += HandleOnHeavyAttackEvent;
            inputReader.SheathUnsheathSword += HandleSheathEvent;
            inputReader.AimHoldEvent += HandleOnAimHoldEvent;
            inputReader.AimHoldCancelEvent += HandleOnAimHoldCancelEvent;
            inputReader.WeaponReturnEvent += HandleOnWeaponReturn;
            inputReader.RollEvent += HandleOnRollEvent;
            inputReader.TargetSelectEvent += HandleOnTargetSelect;
        }
        protected void RemoveMethodsFromEvents()
        {
            inputReader.TargetEvent -= HandleOnTargetEvent;
            inputReader.SprintHoldEvent -= HandleOnSprintHoldEvent;
            inputReader.SprintHoldCanceledEvent -= HandleOnSprintHoldCancelEvent;
            inputReader.SprintEvent -= HandleOnSprintEvent;
            inputReader.LightAttackEvent -= HandleOnLightAttackEvent;
            inputReader.HeavyAttackEvent -= HandleOnHeavyAttackEvent;
            inputReader.SheathUnsheathSword -= HandleSheathEvent;
            inputReader.AimHoldEvent -= HandleOnAimHoldEvent;
            inputReader.AimHoldCancelEvent -= HandleOnAimHoldCancelEvent;
            inputReader.WeaponReturnEvent -= HandleOnWeaponReturn;
            inputReader.RollEvent -= HandleOnRollEvent;
            inputReader.TargetSelectEvent -= HandleOnTargetSelect;
        }
        protected abstract void HandleOnTargetEvent();
        //protected abstract void HandleOnSprintHoldEvent();
        //protected abstract void HandleOnSprintHoldCancelEvent();
        //protected abstract void HandleOnSprintEvent();
        protected abstract void HandleOnLightAttackEvent();
        protected abstract void HandleOnHeavyAttackEvent();
        protected abstract void HandleSheathEvent();
        protected virtual void HandleOnSprintHoldEvent()
        {
            IsSprintHold = true;
            animationController.Sprint(true);
        }

        protected virtual void HandleOnSprintHoldCancelEvent()
        {
            IsSprintHold = false;
            if (IsSprint) return;
            animationController.Sprint(false);
        }

        protected virtual void HandleOnSprintEvent()
        {
            IsSprint = true;
            animationController.Sprint(true);
        }

        protected virtual void HandleSprintControl()
        {
            if (IsSprint && movement.Velocity.sqrMagnitude < 0.1f)
            {
                IsSprint = false;
                if (IsSprintHold) return;
                animationController.Sprint(false);
            }
        }

        protected virtual void HandleOnRollEvent()
        {
            if(stateMachine.stamina.UseStamina(movement.RollStaminaCost))
            {
                stateMachine.isRoll = true;
            }
        }
        protected virtual void HandleOnAimHoldEvent() { }

        protected virtual void HandleOnAimHoldCancelEvent() { }

        protected virtual void HandleOnWeaponReturn() { }

        protected virtual void HandleOnTargetSelect(Vector2 selectDir) { }


    }
}
