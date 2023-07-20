using Combat;
using UnityEngine;

namespace States
{
    public class PlayerSwordTargetState : PlayerCombatTargetState
    {
        public PlayerSwordTargetState(PlayerStateMachine player, Weapon weapon = Weapon.Sword, bool autoStateChange = false) : base(player, weapon, autoStateChange)
        {
        }
        public override void Enter()
        {
            if (stateMachine.PreviousState == stateMachine.UnarmedTargetState || stateMachine.PreviousState == stateMachine.ReturnSwordState || stateMachine.PreviousState == stateMachine.AimState || stateMachine.PreviousState == stateMachine.RollState)
            {
                if (!targetableCheck.TryTransferTarget())
                {
                    stateMachine.ChangeState(stateMachine.SwordFreeState);
                    return;
                }
            }
            if (stateMachine.PreviousState != stateMachine.SwordFreeState && stateMachine.PreviousState != stateMachine.ReturnSwordState &&
                stateMachine.PreviousState != stateMachine.AimState && stateMachine.PreviousState != stateMachine.AimState && stateMachine.PreviousState != stateMachine.RollState) //else if
            {
                animationController.PlayUnsheathSword();
                animationController.TargetCombat(Weapon.Sword, false);
            }
            else
            {
                animationController.TargetCombat(Weapon.Sword, true);
            }
            base.Enter();
            if (stateMachine.PreviousState == stateMachine.RollState && stateMachine.RollState.IsAttack)
            {
                LightAttack();
            }
        }
        public override void Exit()
        {
            animationController.ResetCombatBools();
            base.Exit();
        }
        protected override void StateTickActions(float deltaTime)
        {
            if (animationController.IsUnsheathSheathAnimPlaying)
                return;
            base.StateTickActions(deltaTime);
        }
        protected override void HandleSheathEvent()
        {
            if (animationController.IsAttackPlaying) return;
            stateMachine.ChangeState(stateMachine.UnarmedTargetState);
        }

        protected override void HandleOnTargetEvent()
        {
            stateMachine.ChangeState(stateMachine.SwordFreeState);
        }
        protected override void HandleOnAimHoldEvent()
        {
            stateMachine.ChangeState(stateMachine.AimState);
        }
        protected override void HandleOnTargetSelect(Vector2 selectDir)
        {
            targetableCheck.ChangeTarget(selectDir);
        }
    }

}
