using Unity.VisualScripting;
using UnityEngine;
using static System.TimeZoneInfo;

namespace PlayerController
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private float _animatorDumpTime = 0.1f;
        [SerializeField] Animator _anim;

        public bool IsSprint = false;

        private readonly int _targetSwordBlendTreeHash = Animator.StringToHash("TargetSwordBlendTree");
        private readonly int _freeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");

        private readonly int _freeLookForwardHash = Animator.StringToHash("FreeLookForward");
        private readonly int _freeLookRightHash = Animator.StringToHash("FreeLookRight");

        private readonly int _unarmedForward = Animator.StringToHash("UnarmedForward");

        private readonly int _isSprintHash = Animator.StringToHash("IsSprint");

        private readonly int _targetforwardHash = Animator.StringToHash("TargetForward");
        private readonly int _targetrightHash = Animator.StringToHash("TargetRight");

        private readonly int _targetBoolHash = Animator.StringToHash("IsTarget");
        private readonly int _freeBoolHash = Animator.StringToHash("IsFree");

        public void PlaySetFreeLookBlend()
        {
            _anim.Play(_freeLookBlendTreeHash);
        }
        public void PlaySetBoolsCombatTargetBlendSetBools()
        {
            _anim.Play(_targetSwordBlendTreeHash); //Virtual target camera get set by this anim.
            _anim.SetBool(_targetBoolHash, true);
            _anim.SetBool(_freeBoolHash, false);
        }
        public void SetBoolsCombatFree()
        {
            _anim.SetBool(_targetBoolHash, false);
            _anim.SetBool(_freeBoolHash, true);
        }
        public void CancelTargetBools()
        {
            _anim.SetBool(_targetBoolHash, false);
            _anim.SetBool(_freeBoolHash, false);
        }
        public void UnarmedModeMovement(Vector2 dirVector)
        {
            _anim.SetFloat(_unarmedForward, dirVector.magnitude, _animatorDumpTime, Time.deltaTime);
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
        public void Sprint(bool isSprint)
        {
            _anim.SetBool(_isSprintHash, isSprint);
            IsSprint = isSprint;
        }
        public void PlayAttack(string attackString, float transitionTime)
        {
            _anim.CrossFadeInFixedTime(attackString, transitionTime);
            // _anim.Play(attackString);
        }

        
    }
}
