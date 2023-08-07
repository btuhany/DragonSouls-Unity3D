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
            stateMachine.animController.SetIdleRunLocomotionSpeed(0.0f, 0f);           
            animationController.PlayIdleRunBlend(config.IdleRunAnimTransitionTime);
            animationController.SetIdleRunLocomotionSpeed(0f, config.AnimationDampTime);
        }

        public override void Exit()
        {
        }

        public override void Tick(float deltaTime)
        {
            if (stateMachine.IsPlayerInRange(config.MinIdleRange))
            {
                stateMachine.ChangeState(stateMachine.chaseState);
            }
                
        }
    }

}
