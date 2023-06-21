using PlayerControllers;
using System;
using UnityEngine;
namespace States
{
    public class PlayerTargetState : PlayerBaseState
    {
        public PlayerTargetState(PlayerControl player) : base(player)
        {

        }

        public override void Enter()
        {
            inputReader.TargetEvent += HandleOnTargetEvent;
        }

        public override void Exit()
        {
            inputReader.TargetEvent -= HandleOnTargetEvent;
        }

        public override void Tick(float deltaTime)
        {

        }
        private void HandleOnTargetEvent()
        {
            stateMachine.ChangeState(player.FreeLookPlayerState);
        }
    }
}
