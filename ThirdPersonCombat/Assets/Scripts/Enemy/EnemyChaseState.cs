using System.Collections;
using UnityEngine;
namespace States
{
    public class EnemyChaseState : EnemyBaseState
    {
        private float _timeCounter = 0f;

        float _chaseToTargetChangeRange;
        int _randomNumberGen;
        public EnemyChaseState(EnemyStateMachine enemy) : base(enemy)
        {
        }

        public override void Enter()
        {
            navmeshAgent.isStopped = false;
            _timeCounter = 0f;
            _chaseToTargetChangeRange = Random.Range(config.ChaseToTargetChangeMinRange, config.ChaseToTargetChangeMaxRange);
            _randomNumberGen = Random.Range(1, 101);
            navmeshAgent.SetDestination(PlayerStateMachine.Instance.transform.position);
            animationController.PlayIdleRunBlend(config.IdleRunAnimTransitionTime);
        }

        public override void Exit()
        {
           
        }

        public override void Tick(float deltaTime)
        {
            _timeCounter += deltaTime;
            if(_timeCounter > stateMachine.Config.ChasePointRefreshTime)
            {
                navmeshAgent.SetDestination(PlayerStateMachine.Instance.transform.position);
                _timeCounter = 0;
            }
            
            animationController.SetIdleRunLocomotionSpeed(navmeshAgent.velocity.magnitude / navmeshAgent.speed, config.AnimationDampTime);

            if(_randomNumberGen <= 100 * config.ChaseToTargetChangeProbability)
            {
                if (stateMachine.IsPlayerInRange(_chaseToTargetChangeRange))
                    stateMachine.ChangeState(stateMachine.TargetState);
            }
            else
            {
                if (stateMachine.IsPlayerInRange(config.ChaseToAttackChangeRange))
                    stateMachine.ChangeState(stateMachine.AttackState);
            }

            if(!stateMachine.IsPlayerInRange(config.ChaseToIdleChangeRange))
                stateMachine.ChangeState(stateMachine.IdleState);
          
        }


    }

}
