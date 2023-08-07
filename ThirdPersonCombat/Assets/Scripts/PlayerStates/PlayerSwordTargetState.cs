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
            if (stateMachine.PreviousState == stateMachine.unarmedTargetState || stateMachine.PreviousState == stateMachine.returnSwordState || stateMachine.PreviousState == stateMachine.aimState || stateMachine.PreviousState == stateMachine.rollState)
            {
                if (!targetableCheck.TryTransferTarget())
                {
                    stateMachine.ChangeState(stateMachine.swordFreeState);
                    return;
                }
            }
            if (stateMachine.PreviousState != stateMachine.swordFreeState && stateMachine.PreviousState != stateMachine.returnSwordState &&
                stateMachine.PreviousState != stateMachine.aimState && stateMachine.PreviousState != stateMachine.aimState && stateMachine.PreviousState != stateMachine.rollState) //else if
            {
                animationController.PlayUnsheathSword();
                animationController.TargetCombat(Weapon.Sword, false);
            }
            else
            {
                animationController.TargetCombat(Weapon.Sword, true);
            }
            base.Enter();
            if (stateMachine.PreviousState == stateMachine.rollState && stateMachine.rollState.IsAttack)
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
            if (!targetableCheck.IsTargetInRange())
            {
                stateMachine.ChangeState(stateMachine.swordFreeState);
                return;
            }
            base.StateTickActions(deltaTime);
        }
        protected override void HandleSheathEvent()
        {
            if (animationController.IsAttackPlaying) return;
            stateMachine.ChangeState(stateMachine.unarmedTargetState);
        }

        protected override void HandleOnTargetEvent()
        {
            stateMachine.ChangeState(stateMachine.swordFreeState);
        }
        protected override void HandleOnAimHoldEvent()
        {
            stateMachine.ChangeState(stateMachine.aimState);
        }
        protected override void HandleOnTargetSelect(Vector2 selectDir)
        {
            targetableCheck.ChangeTarget(selectDir);
        }
    }

}
