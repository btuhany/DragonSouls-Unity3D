using PlayerController;
using UnityEngine;
using Combat;
namespace States
{
    public abstract class PlayerCombatFreeState : PlayerCombatState
    {
        protected bool entryAttack = false;
        protected Weapon weapon;
        public PlayerCombatFreeState(PlayerStateMachine player, Weapon weapon, bool entryAttack, bool autoStateChange = false) : base(player, weapon, autoStateChange)
        {
            this.entryAttack = entryAttack;
            this.weapon = weapon;
        }
        public override void Enter()
        {
            base.Enter();
            //animationController.PlaySetBoolsCombatFree(_weapon);
            if (stateMachine.PreviousState == stateMachine.unarmedTargetState) return;
            if (stateMachine.PreviousState == stateMachine.swordTargetState) return;
            if (entryAttack)
            {
                if (inputReader.LastAttackType == AttackType.Light)
                {
                    LightAttack();
                }
                else if (inputReader.LastAttackType == AttackType.Heavy)
                {
                    HeavyAttack();
                }
            }
        }
        public override void Exit()
        {
            animationController.ResetCombatBools();
            base.Exit();
        }
    }
}

