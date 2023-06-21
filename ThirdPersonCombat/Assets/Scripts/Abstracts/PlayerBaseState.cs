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
        protected CharacterController characterController;
        protected Transform mainCamTransform;
        protected InputReader inputReader;
        protected PlayerAnimationController animationController;
        protected PlayerStateMachine stateMachine;
        public PlayerBaseState(PlayerControl player)
        {
            mainCamTransform = Camera.main.transform;
            this.player = player;
            this.transform = player.transform;
            this.characterController = player.CharacterController;
            this.inputReader = player.InputReader;
            this.animationController = player.AnimationController;
            this.stateMachine = player.StateMachine;
        }
    }
}
