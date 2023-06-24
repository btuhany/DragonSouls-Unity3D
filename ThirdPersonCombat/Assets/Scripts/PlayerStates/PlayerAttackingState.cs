using UnityEngine;

namespace States
{
    public class PlayerAttackingState : PlayerBaseState
    {
        AttackType attackType;
        public PlayerAttackingState(PlayerStateMachine player, AttackType attacktype ) : base(player)
        {
        }

        public override void Enter()
        {
            animationController.PlayAttack(combat.Attacks[0].animationName, combat.Attacks[0].animationTransitionDuration);
        }

        public override void Exit()
        {
        }

        public override void Tick(float deltaTime)
        {
        }
    }
}
public enum AttackType
{
    Light,
    Heavy
}