using PlayerController;
using UnityEngine;
namespace States
{
    public abstract class PlayerCombatFreeState : PlayerCombatState
    {
        private bool _entryAttack = false;
        public PlayerCombatFreeState(PlayerStateMachine player, Weapon weapon, bool entryAttack, bool autoStateChange = false) : base(player, weapon, autoStateChange)
        {
            _entryAttack = entryAttack;
        }

        protected override void StateEnterActions()
        {
            animationController.SetBoolsCombatFree();
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

