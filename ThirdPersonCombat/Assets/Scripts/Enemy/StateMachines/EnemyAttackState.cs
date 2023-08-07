using Combat;
using TMPro;
using UnityEngine;
namespace States
{
    public class EnemyAttackState : EnemyBaseState
    {
        private EnemyCombatController _combat;
        private float _animationTimeCounter = 0f;
        private float _timeCounter = 0f;
        private float _randomWaitTime = 0f;
        public EnemyAttackState(EnemyStateMachine enemy) : base(enemy)
        {
            _combat = enemy.combatController;
        }

        public override void Enter()
        {
            _randomWaitTime = Random.Range(config.AttackMinWaitTime, config.AttackMaxWaitTime);
            stateMachine.animController.SetIdleRunLocomotionSpeed(0.0f, 0f);
            RandomAttack();
            _animationTimeCounter = 0f;
            _timeCounter = 0f;
            LookToPlayer(0.4f);
        }

        public override void Exit()
        {

        }

        public override void Tick(float deltaTime)
        {
            _animationTimeCounter += deltaTime;
            if (_animationTimeCounter > _combat.CurrentAttack.attackDuration)
            {
                LookToPlayer(deltaTime);
                _timeCounter += deltaTime;
                if (_timeCounter < _randomWaitTime)
                {
                    if (!IsTurnedToPlayer())
                    {
                        stateMachine.animController.SetIdleRunLocomotionSpeed(0.05f, 0f);
                    }
                    else
                    {
                        stateMachine.animController.SetIdleRunLocomotionSpeed(0.0f, 0f);
                        if (!stateMachine.IsPlayerInRange(config.AttackToChaseChangeRange))
                        {
                            stateMachine.ChangeState(stateMachine.chaseState);
                            return;
                        }
                        else if (!stateMachine.IsPlayerInRange(config.AttackToTargetChangeRange))
                        {
                            stateMachine.ChangeState(stateMachine.targetState);
                            return;
                        }
                    }


                }
                else
                {

                    RandomAttack();
                    _timeCounter = 0f;
                    _randomWaitTime = Random.Range(config.AttackMinWaitTime, config.AttackMaxWaitTime);
                }
            }

        }

        void RandomAttack()
        {
            _animationTimeCounter = 0f;
            Attack randomAttack = _combat.Attacks[Random.Range(0, _combat.Attacks.Length)];
            _combat.CurrentAttack = randomAttack;
            animationController.PlayAttack(randomAttack.animationName, randomAttack.transitionDuration);
            stateMachine.forceReceiver.AddForce();
        }

        private bool IsTurnedToPlayer()
        {
            Vector3 dir = PlayerStateMachine.Instance.transform.position - stateMachine.transform.position;
            dir.y = 0f;
            float similarity = Vector3.Dot(dir.normalized, stateMachine.transform.forward);
            if (similarity > 0.99f)
                return true;
            return false;
        }

        private void LookToPlayer(float deltaTime)
        {
            Vector3 playerPos = PlayerStateMachine.Instance.transform.position;
            Vector3 dir = playerPos - stateMachine.transform.position;
            dir.y = 0f;
            movement.LookRotation(dir, _randomWaitTime * config.LookRotationLerpTimeMultiplier, deltaTime);
        }
    }

}