using Inputs;
using PlayerController;
using UnityEngine;
using Movement;

namespace States
{
    public abstract class PlayerBaseState : State
    {
        protected bool isSprintHold = false;
        protected bool isSprint = false;
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
            this.animationController = player.AnimationController;
            this.targetableCheck = player.TargetableCheck;
            this.movement = player.Movement;
            this.forceReciver = player.ForceReceiver;
        }
        public override void Enter()
        {
            AddMethodsToEvents();
        }
        public override void Exit()
        {
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
        protected void AddMethodsToEvents()
        {
            inputReader.TargetEvent += HandleOnTargetEvent;
            inputReader.SprintHoldEvent += HandleOnSprintHoldEvent;
            inputReader.SprintHoldCanceledEvent += HandleOnSprintHoldCancelEvent;
            inputReader.SprintEvent += HandleOnSprintEvent;
            inputReader.LightAttackEvent += HandleOnLightAttackEvent;
            inputReader.HeavyAttackEvent += HandleOnHeavyAttackEvent;
            inputReader.SheathUnsheathSword += HandleSheathEvent;
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
            isSprintHold = true;
            animationController.Sprint(true);
        }

        protected virtual void HandleOnSprintHoldCancelEvent()
        {
            isSprintHold = false;
            if (isSprint) return;
            animationController.Sprint(false);
        }

        protected virtual void HandleOnSprintEvent()
        {
            isSprint = true;
            animationController.Sprint(true);
        }

        protected virtual void HandleSprintControl()
        {
            if (isSprint && movement.Velocity.sqrMagnitude < 0.1f)
            {
                isSprint = false;
                if (isSprintHold) return;
                animationController.Sprint(false);
            }
        }
    }
}
