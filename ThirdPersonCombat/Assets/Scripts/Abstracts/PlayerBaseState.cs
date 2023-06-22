using Inputs;
using PlayerController;
using PlayerControllers;
using UnityEngine;

namespace States
{
    public abstract class PlayerBaseState : State
    {
        protected PlayerControl player;
        protected Transform transform;
        protected Transform mainCamTransform;
        protected InputReader inputReader;
        protected PlayerAnimationController animationController;
        protected PlayerStateMachine stateMachine;
        protected TargetableCheck targetableCheck;
        protected MovementController movement;
        public PlayerBaseState(PlayerControl player)
        {
            mainCamTransform = Camera.main.transform;
            this.player = player;
            this.transform = player.transform;
            this.inputReader = player.InputReader;
            this.animationController = player.AnimationController;
            this.stateMachine = player.StateMachine;
            this.targetableCheck = player.TargetableCheck;
            this.movement = player.Movement;
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
    }
}
