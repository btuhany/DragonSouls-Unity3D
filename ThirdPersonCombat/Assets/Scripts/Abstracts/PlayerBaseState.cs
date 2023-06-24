using Inputs;
using PlayerController;
using UnityEngine;

namespace States
{
    public abstract class PlayerBaseState : State
    {
        protected Transform transform;
        protected Transform mainCamTransform;
        protected InputReader inputReader;
        protected PlayerAnimationController animationController;
        protected PlayerStateMachine player;
        protected TargetableCheck targetableCheck;
        protected MovementController movement;
        protected CombatController combat;

        public PlayerBaseState(PlayerStateMachine player)
        {
            mainCamTransform = Camera.main.transform;
            this.player = player;
            this.transform = player.transform;
            this.inputReader = player.InputReader;
            this.animationController = player.AnimationController;
            this.targetableCheck = player.TargetableCheck;
            this.movement = player.Movement;
            this.combat = player.CombatController;
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
