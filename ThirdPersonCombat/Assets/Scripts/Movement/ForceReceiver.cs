using UnityEngine;

namespace Movement
{
    public class ForceReceiver : MonoBehaviour
    {
        [SerializeField] private float _gravityScale = 1;
        [SerializeField] private CharacterController _characterController;

        [Header("Ground Check")]
        [SerializeField] private Transform _groundCheckTransform;
        [SerializeField] private float _checkRadius;
        [SerializeField] private LayerMask _groundMask;

        private Vector3 _verticalVelocity = Vector3.zero;

        private Vector3 _currentVelocity = Vector3.zero;

        private Vector3 _impact;
        private float _impactSmoothTime = 0.1f;

   
        private readonly float _gravity = Physics.gravity.y;
        private bool _isGrounded => Physics.CheckSphere(_groundCheckTransform.position, _checkRadius, _groundMask);
        private void Update()
        {
            if (_impact.sqrMagnitude > 0.07f)
                _impact = Vector3.SmoothDamp(_impact, Vector3.zero, ref _currentVelocity ,_impactSmoothTime);
            else
                _impact = Vector3.zero;


            _verticalVelocity.y += _gravity * Time.deltaTime * _gravityScale;

            if (_isGrounded && _verticalVelocity.y < 0f)
                _verticalVelocity.y = -2f;
            _characterController.Move((_verticalVelocity + _impact) * Time.deltaTime);
        }

        public void AddForce(Vector3 force, float lerpTime)
        {
            _impact = force;
            _impactSmoothTime = lerpTime;
        }

    }
}
