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
            if (stateMachine.isDead)
                stateMachine.ChangeState(stateMachine.deadState);
            stateMachine.sound.PlayGruntsSFX(true);
            navmeshAgent.isStopped = false;
            navmeshAgent.updatePosition = false;
            _timeCounter = 0f;
            _chaseToTargetChangeRange = Random.Range(config.ChaseToTargetChangeMinRange, config.ChaseToTargetChangeMaxRange);
            _randomNumberGen = Random.Range(1, 101);
            navmeshAgent.destination = PlayerStateMachine.Instance.transform.position;
            animationController.PlayIdleRunBlend(config.IdleRunAnimTransitionTime);
        }

        public override void Exit()
        {
            stateMachine.sound.PlayGruntsSFX(false);
            navmeshAgent.isStopped = true;
            navmeshAgent.updatePosition = true;
            navmeshAgent.ResetPath();
            navmeshAgent.velocity = Vector3.zero;
        }

        public override void Tick(float deltaTime)
        {
            _timeCounter += deltaTime;
            if (_timeCounter > stateMachine.config.ChasePointRefreshTime)
            {
                navmeshAgent.destination = PlayerStateMachine.Instance.transform.position;
                _timeCounter = 0;
            }

            if (!stateMachine.forceReceiver.isGrounded) return;
            if (stateMachine.forceReceiver.isImpacted) return;
            movement.Move(navmeshAgent.desiredVelocity.normalized, config.ChaseSpeed, deltaTime);
            navmeshAgent.velocity = movement.Velocity;
            Vector3 faceDir = movement.Velocity;
            faceDir.y = 0f;
            movement.LookRotation(faceDir, 4.2f, deltaTime);

            animationController.SetIdleRunLocomotionSpeed(movement.Velocity.magnitude / config.ChaseSpeed, config.AnimationDampTime);

            if (_randomNumberGen <= 100 * config.ChaseToTargetChangeProbability)
            {
                if (stateMachine.IsPlayerInRange(_chaseToTargetChangeRange))
                {
                    stateMachine.ChangeState(stateMachine.targetState);
                    return;
                }

            }
            else
            {
                if (stateMachine.IsPlayerInRange(config.ChaseToAttackChangeRange))
                {
                    stateMachine.ChangeState(stateMachine.attackState);
                    return;
                }
            }

            if (Vector3.Distance(stateMachine.transform.position, PlayerStateMachine.Instance.transform.position) > config.ChaseToIdleChangeRange)
            {
                stateMachine.ChangeState(stateMachine.idleState);
                return;
            }

            if (PlayerStateMachine.Instance.isInvisible)
                stateMachine.ChangeState(stateMachine.idleState);
        }


    }

}
