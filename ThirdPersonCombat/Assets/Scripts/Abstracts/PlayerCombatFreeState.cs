using PlayerController;
using UnityEngine;
namespace States
{
    public abstract class PlayerCombatFreeState : PlayerCombatState
    {
        private bool _entryAttack = false;
        private Weapon _weapon;
        public PlayerCombatFreeState(PlayerStateMachine player, Weapon weapon, bool entryAttack, bool autoStateChange = false) : base(player, weapon, autoStateChange)
        {
            _entryAttack = entryAttack;
            _weapon = weapon;
        }

        protected override void StateEnterActions()
        {
            animationController.PlaySetBoolsCombatFree(_weapon);

            if (stateMachine.PreviousState == stateMachine.UnarmedTargetState) return;
            if (stateMachine.PreviousState == stateMachine.SwordFreeState) return;
            if (_entryAttack)
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

        protected override void StateExitActions()
        {
            animationController.ResetCombatBools();
        }

    }
}

