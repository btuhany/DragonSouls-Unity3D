using States;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyControllers
{
    public class EnemyForceReceiver : MonoBehaviour
    {
        [SerializeField] private bool _disableImpact = false;

        private CharacterController characterController;
        [SerializeField] private float _gravityScale = 1;
        [SerializeField] private float _impactMagnitude = 5f;
        [SerializeField] private float _impactLerpTime = 0.1f;

        [SerializeField] private Transform _enemyCheckTransform;
        [SerializeField] private float _enemyCheckRadius;
        [SerializeField] private LayerMask _playerLayer;

        [Header("Ground Check")]
        [SerializeField] private Transform _groundCheckTransform;
        [SerializeField] private float _checkRadius;
        [SerializeField] private LayerMask _groundMask;

        private Vector3 _currentVelocity = Vector3.zero;
        private Vector3 _impact = Vector3.zero;

        private Vector3 _verticalVelocity = Vector3.zero;
        public bool isGrounded => Physics.CheckSphere(_groundCheckTransform.position, _checkRadius, _groundMask);
        public bool isImpacted;
        private readonly float _gravity = Physics.gravity.y;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }
        private void Update()
        {
            if(!_disableImpact)
            {
                if (_impact.sqrMagnitude > 0.07f)
                {
                    _impact = Vector3.SmoothDamp(_impact, Vector3.zero, ref _currentVelocity, _impactLerpTime);
                }
                else if(isImpacted)
                {
                    _impact = Vector3.zero;
                    isImpacted = false;
                }
                characterController.Move(_impact * Time.deltaTime);
            }

            _verticalVelocity.y += _gravity * Time.deltaTime * _gravityScale;
            if (isGrounded && _verticalVelocity.y < 0f)
                _verticalVelocity.y = -2f;

            characterController.Move(_verticalVelocity * Time.deltaTime);
        }

        public void AddForce()
        {
            if (Physics.CheckSphere(_enemyCheckTransform.position, _enemyCheckRadius, _playerLayer)) return;
            if (Vector3.Distance(PlayerStateMachine.Instance.transform.position, transform.position) < 1.2f) return;
            _impact = transform.forward * _impactMagnitude;
            isImpacted = true;
        }

        public void AddForce(Vector3 impact)
        {
            _impact = impact;
            isImpacted = true;
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (_impact != Vector3.zero)
                {
                    _impact = Vector3.zero;
                    isImpacted = false;
                }
            }
        }
    }

}
