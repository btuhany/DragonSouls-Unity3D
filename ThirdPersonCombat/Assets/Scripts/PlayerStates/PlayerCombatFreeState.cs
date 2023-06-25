using PlayerController;
using UnityEngine;
namespace States
{
    public abstract class PlayerCombatFreeState : PlayerCombatState
    {
        public PlayerCombatFreeState(PlayerStateMachine player, Weapon weapon , bool autoStateChange = false) : base(player, weapon , autoStateChange)
        {

        }

        protected override void StateEnterActions()
        {
            animationController.SetBoolsCombatFree();
            if(inputReader.LastAttackType == AttackType.Light)
            {
                LightAttack();
            }
            else if(inputReader.LastAttackType == AttackType.Heavy)
            {
                HeavyAttack();
            }
        }

        protected override void StateExitActions()
        {
            animationController.ResetCombatBools();
        }
    }
}

