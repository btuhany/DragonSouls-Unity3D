using UnityEngine;

namespace PlayerController
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private float _animatorDumpTime = 0.1f;
        [SerializeField] Animator _anim;

        private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
        public void FreeLookMovementAnim(float value)
        {
            _anim.SetFloat(FreeLookSpeedHash, value, _animatorDumpTime, Time.deltaTime);
        }
    }
}
