using Inputs;
using UnityEngine;

namespace States
{
    public class EnemyTargetState : EnemyBaseState
    {
        private Transform _playerTargetTransform;
        private float _rightValue = 0f;
        private float _forwardValue = 0f;

        public EnemyTargetState(EnemyStateMachine enemy) : base(enemy)
        {
        }

        public override void Enter()
        {
            _playerTargetTransform = PlayerStateMachine.Instance.transform;
            _rightValue = 2f;
        }

        public override void Exit()
        {
        }

        public override void Tick(float deltaTime)
        {
            //stateMachine.Movement.LookRotation(movement.TargetRelativeMotionVector(_playerTargetTransform.position), 20f, deltaTime);
            //stateMachine.transform.RotateAround(_playerTargetTransform.position, Vector3.up, stateMachine.Config.TargetedMovementSpeed);
        }
        private Vector3 MotionVectorAroundTarget()
        {
            //Character always looks to target
            Vector3 motion = Vector3.zero;
            motion += stateMachine.transform.right * _rightValue;
            motion += stateMachine.transform.forward * _forwardValue;
            return motion;
        }
    }
}
