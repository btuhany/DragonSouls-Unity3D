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
            _combat = enemy.Combat;
        }

        public override void Enter()
        {
            _randomWaitTime = Random.Range(config.MinWaitTime, config.MaxWaitTime);
            RandomAttack();
        }

        public override void Exit()
        {
            _animationTimeCounter = 0f;  
            _timeCounter = 0f;
         }

        public override void Tick(float deltaTime)
        {
            _animationTimeCounter += deltaTime;
            if (_animationTimeCounter > _combat.CurrentAttack.attackDuration)
            {
                _timeCounter += deltaTime;
                if (_timeCounter < _randomWaitTime)
                {
                    if (!TurnToPlayer(deltaTime))
                        stateMachine.AnimationController.SetLocomotionSpeed(0.05f, 0f);
                    else
                        stateMachine.AnimationController.SetLocomotionSpeed(0.0f, 0.1f);

                    return;
                }
                RandomAttack();
                _animationTimeCounter = 0f;
                _timeCounter = 0f;
                _randomWaitTime = Random.Range(config.MinWaitTime, config.MaxWaitTime);
            }
        }

        void RandomAttack()
        {
            Attack randomAttack = _combat.Attacks[Random.Range(0, _combat.Attacks.Length)];
            _combat.CurrentAttack = randomAttack;
            animationController.PlayAttack(randomAttack.animationName, randomAttack.transitionDuration);
        }

        private bool TurnToPlayer(float deltaTime)
        {
            Vector3 playerPos = PlayerStateMachine.Instance.transform.position;
            movement.LookRotation(playerPos - stateMachine.transform.position, _randomWaitTime * config.LookRotationLerpTimeMultiplier, deltaTime);
            float similarity = Quaternion.Dot(PlayerStateMachine.Instance.transform.rotation, stateMachine.transform.rotation);
            if (similarity < 0.1f)
                return true;
            return false;
        }
    }

}