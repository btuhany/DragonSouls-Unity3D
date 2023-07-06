using UnityEngine;
namespace States
{
    public class EnemyIdleState : EnemyBaseState
    {
        public EnemyIdleState(EnemyStateMachine enemy) : base(enemy)
        {
        }

        public override void Enter()
        {
            animationController.PlayIdleRunBlend(config.IdleRunAnimTransitionTime);
            animationController.SetLocomotionSpeed(0f, config.AnimationDampTime);
        }

        public override void Exit()
        {
        }

        public override void Tick(float deltaTime)
        {
            if (stateMachine.IsPlayerInRange(config.MinIdleRange))
            {
                stateMachine.ChangeState(stateMachine.enemyChaseState);
            }
                
        }
    }

}
