using UnityEngine;

namespace PlayerController
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private float _animatorDumpTime = 0.1f;
        [SerializeField] Animator _anim;

        private readonly int _freeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
        private readonly int _freeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
        private readonly int _targetBlendTreeHash = Animator.StringToHash("TargetBlendTree");
        public void FreeLookMovementAnim(float value)
        {
            _anim.SetFloat(_freeLookSpeedHash, value, _animatorDumpTime, Time.deltaTime);
        }
        public void PlayFreeLook()
        {
            _anim.Play(_freeLookBlendTreeHash);
        }
        public void PlayTarget()
        {
            _anim.Play(_targetBlendTreeHash);
        }
    }
}
