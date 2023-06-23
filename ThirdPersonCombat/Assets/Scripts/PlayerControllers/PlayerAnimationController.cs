using UnityEngine;

namespace PlayerController
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private float _animatorDumpTime = 0.1f;
        [SerializeField] Animator _anim;

        private readonly int _freeLookForwardHash = Animator.StringToHash("FreeLookForward");
        private readonly int _freeLookRightHash = Animator.StringToHash("FreeLookRight");
        private readonly int _freeLookRunHash = Animator.StringToHash("FreeLookRun");
        private readonly int _freeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
        private readonly int _targetSwordBlendTreeHash = Animator.StringToHash("TargetSwordBlendTree");

        private readonly int _targetforwardHash = Animator.StringToHash("TargetForward");
        private readonly int _targetrightHash = Animator.StringToHash("TargetRight");

        public void PlayFreeLook()
        {
            _anim.Play(_freeLookBlendTreeHash);
        }
        public void PlayTarget()
        {
            _anim.Play(_targetSwordBlendTreeHash);
        }
        public void TargetMovementBlendTree(Vector2 dirVector)
        {
            _anim.SetFloat(_targetforwardHash, dirVector.y);
            _anim.SetFloat(_targetrightHash, dirVector.x);
        }
        public void FreeLookMovementBlendTree(Vector2 dirVector)
        {
            float yAxisVelocityMagnitude = Mathf.Abs(dirVector.y);
            //float xAxiisVelocityMagnite = Mathf.Abs(dirVector.x);

            _anim.SetFloat(_freeLookForwardHash, yAxisVelocityMagnitude, _animatorDumpTime, Time.deltaTime);
            _anim.SetFloat(_freeLookRightHash, dirVector.x, _animatorDumpTime, Time.deltaTime);
        }
    }
}
