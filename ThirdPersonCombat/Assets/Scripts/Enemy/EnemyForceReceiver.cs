using States;
using UnityEngine;

namespace EnemyControllers
{
    public class EnemyForceReceiver : MonoBehaviour
    {
        [SerializeField] private float _impactMagnitude = 5f;
        [SerializeField] private float _impactLerpTime = 0.1f;
        [SerializeField] private CharacterController _characterController;

        [SerializeField] private Transform _enemyCheckTransform;
        [SerializeField] private float _enemyCheckRadius;
        [SerializeField] private LayerMask _enemyCheckGroundMask;

        private Vector3 _currentVelocity = Vector3.zero;
        private Vector3 _impact;
        private void Update()
        {
            if (_impact.sqrMagnitude > 0.07f)
                _impact = Vector3.SmoothDamp(_impact, Vector3.zero, ref _currentVelocity, _impactLerpTime);
            else
            {
                _impact = Vector3.zero;
            }
            transform.position += _impact * Time.deltaTime;
        }

        public void AddForce()
        {
            if (Physics.CheckSphere(_enemyCheckTransform.position, _enemyCheckRadius, _enemyCheckGroundMask)) return;
            if (Vector3.Distance(PlayerStateMachine.Instance.transform.position, transform.position) < 1.2f) return;
            _impact = transform.forward * _impactMagnitude;
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (_impact != Vector3.zero)
                    _impact = Vector3.zero;
            }
        }
    }

}
