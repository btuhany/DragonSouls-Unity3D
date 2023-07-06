using UnityEngine;
namespace States
{
    public class EnemyChaseState : EnemyBaseState
    {
        private float _timeCounter = 0f;

        public EnemyChaseState(EnemyStateMachine enemy) : base(enemy)
        {
        }

        public override void Enter()
        {
            navmeshAgent.SetDestination(PlayerStateMachine.Instance.transform.position);
            animationController.PlayIdleRunBlend(config.IdleRunAnimTransitionTime);
        }

        public override void Exit()
        {
            _timeCounter = 0f;
        }

        public override void Tick(float deltaTime)
        {
            _timeCounter += deltaTime;
            if(_timeCounter > stateMachine.Config.DestinationPointRefreshTime)
            {
                navmeshAgent.SetDestination(PlayerStateMachine.Instance.transform.position);
                _timeCounter = 0;
            }
            
            animationController.SetLocomotionSpeed(navmeshAgent.velocity.magnitude / navmeshAgent.speed, config.AnimationDampTime);
            if (stateMachine.IsPlayerInRange(config.MaxAttackDistance))
                stateMachine.ChangeState(stateMachine.enemyAttackState);
        }
    }

}
