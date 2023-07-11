using Inputs;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace States
{
    public class EnemyTargetState : EnemyBaseState
    {
        private int _rightValue = 0;
        private int _forwardValue = 0;
        private float _randomDirChangeTime = 0f;
        private float dirChangeTimeCounter = 0f;
        private float _randomApproachPlayerTime = 0f;
        private float _approachPlayerTimeCounter = 0f;
        private float _movementSpeedMultiplier = 1f;
        private float _speedUpTimeCounter = 0f;
        private float _speedUpTime = 0f;
        private bool _isApproach = false;
        private bool _isMovingBack = false;

        private EnemyMovementController _movement;
        private EnemyConfig _config;
        private Transform _playerTargetTransform;
        private float _playerDistance;

        public EnemyTargetState(EnemyStateMachine enemy) : base(enemy)
        {
            _movement = enemy.Movement;
            _config = enemy.Config;
            _playerTargetTransform = PlayerStateMachine.Instance.transform;
        }

        public override void Enter()
        {
            _isApproach = false;
            _isMovingBack = false;
            _rightValue = RandomSign();
            dirChangeTimeCounter = 0f;
            _approachPlayerTimeCounter = 0f;
            _speedUpTimeCounter = 0f;
            _forwardValue = 0;
            _movementSpeedMultiplier = 1f;
            _randomDirChangeTime = Random.Range(_config.TargetMinRightDirChangeTime, _config.TargetMaxRightDirChangeTime);
            _randomApproachPlayerTime = Random.Range(_config.TargetMinApproachPlayerTime, _config.TargetMaxApproachPlayerTime);
            _speedUpTime = Random.Range(_config.TargetMinSpeedUpTime, _config.TargetMaxSpeedUpTime);
            _playerDistance = Vector3.Distance(_playerTargetTransform.position, stateMachine.transform.position);
        }

        public override void Exit()
        {
        }

        public override void Tick(float deltaTime)
        {
            if (!_isApproach)
            {
                dirChangeTimeCounter += deltaTime;
                _approachPlayerTimeCounter += deltaTime;

                if (dirChangeTimeCounter >= _randomDirChangeTime)
                {
                    if(!_isMovingBack && Random.Range(1,101) >= 100 - 100 * _config.TargetBackDirChangeProbility)
                    {
                        _isMovingBack = true;
                        _rightValue = 0;
                        _forwardValue = -1;
                        _randomDirChangeTime = Random.Range(_config.TargetMinBackDirChangeTime, _config.TargetMaxBackDirChangeTime);
                    }
                    else
                    {
                        _isMovingBack = false;
                        _forwardValue = 0;
                        _rightValue = RandomSign();
                        _randomDirChangeTime = Random.Range(_config.TargetMinRightDirChangeTime, _config.TargetMaxRightDirChangeTime);
                    }

                    dirChangeTimeCounter = 0f;
                }

                if (_approachPlayerTimeCounter >= _randomApproachPlayerTime)
                {
                    _isApproach = true;
                    _forwardValue = 1;
                    _rightValue = 0;
                    _approachPlayerTimeCounter = 0f;
                    _approachPlayerTimeCounter = Random.Range(_config.TargetMinApproachPlayerTime, _config.TargetMaxApproachPlayerTime);
                }
            }
            else
            {
                _speedUpTimeCounter += deltaTime;
                if(_speedUpTimeCounter >= _speedUpTime)
                {
                    _movementSpeedMultiplier = Random.Range(_config.TargetMinSpeedUpValue, _config.TargetMaxSpeedUpValue);
                }
            }

            _movement.LookRotation(TargetRelativeMotionVector(_playerTargetTransform.position));

            _movement.Move(MotionVectorAroundTarget(_rightValue), _config.TargetMovementSpeed, deltaTime);

            _playerDistance -= _forwardValue * _config.TargetMovementSpeed * deltaTime * _movementSpeedMultiplier;
            if(_playerDistance < 0.5f ) _playerDistance = 0.5f; 

            //StabilizeDistance(_playerDistance);
        }

        private Vector3 MotionVectorAroundTarget(float rightVal)
        {
            //right value should be in range of [-1,1]
            Vector3 motion = Vector3.zero;
            motion += stateMachine.transform.right * rightVal;
            return motion;
        }

        private Vector3 TargetRelativeMotionVector(Vector3 targetPos)
        {
            Vector3 relativeVector = targetPos - stateMachine.transform.position;
            relativeVector.y = 0f;

            return relativeVector;
        }

        private void StabilizeDistance(float distance)
        {
            float currentDistance = Vector3.Distance(_playerTargetTransform.position, stateMachine.transform.position);
            float difference = distance  - currentDistance;
            Vector3 dif = stateMachine.transform.position - _playerTargetTransform.position;
            stateMachine.transform.position += dif.normalized * difference;
        }

        private int RandomSign()
        {
            int randomNumber = Random.Range(1, 3);
            if (randomNumber == 2)
                return -1;
            return randomNumber;
        }
    }
}
