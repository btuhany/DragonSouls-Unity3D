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
        public bool IsUnsheathAnimPlaying => _anim.GetCurrentAnimatorStateInfo(0).IsTag("Unsheath");


        private readonly int _swordTargetBlendTreeHash = Animator.StringToHash("SwordTargetBlendTree");
        private readonly int _unarmedTargetBlendTreeHash = Animator.StringToHash("UnarmedTargetBlendTree");
        private readonly int _unarmedFreeBlendTreeHash = Animator.StringToHash("UnarmedFreeBlendTree");
        private readonly int _swordFreeBlendTreeHash = Animator.StringToHash("SwordFreeBlendTree");
        private readonly int _freeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");


        private readonly int _freeLookForwardHash = Animator.StringToHash("FreeLookForward");
        private readonly int _freeLookRightHash = Animator.StringToHash("FreeLookRight");
        private readonly int _targetforwardHash = Animator.StringToHash("TargetForward");
        private readonly int _targetrightHash = Animator.StringToHash("TargetRight");

        private readonly int _combatFreeForwardHash = Animator.StringToHash("CombatFreeForward");


        private readonly int _unsheathSwordHash = Animator.StringToHash("UnsheathSword");
        private readonly int _isSprintHash = Animator.StringToHash("IsSprint");
        private readonly int _targetBoolHash = Animator.StringToHash("IsTarget");
        private readonly int _freeBoolHash = Animator.StringToHash("IsFree");

        public void PlaySetFreeLookBlend()
        {
            _anim.Play(_freeLookBlendTreeHash);
        }
        public void PlaySetBoolsCombatTarget(Weapon weapon)
        {
            if(weapon == Weapon.Unarmed)
            {
                _anim.Play(_unarmedTargetBlendTreeHash);
            }
            else if(weapon == Weapon.Sword)
            {
                _anim.Play(_swordTargetBlendTreeHash); //Virtual target camera get set by this anim.
            }
            _anim.SetBool(_targetBoolHash, true);
            _anim.SetBool(_freeBoolHash, false);
        }
        public void PlayUnsheathSword()
        {
            _anim.Play(_unsheathSwordHash);
        }
        public void PlaySetBoolsCombatFree(Weapon weapon)
        {
            if (weapon == Weapon.Unarmed)
            {
                _anim.Play(_unarmedFreeBlendTreeHash);
            }
            else if (weapon == Weapon.Sword)
            {
                _anim.Play(_swordFreeBlendTreeHash); //Virtual target camera get set by this anim.
            }
            _anim.SetBool(_targetBoolHash, false);
            _anim.SetBool(_freeBoolHash, true);
        }
        public void ResetCombatBools()
        {
            _anim.SetBool(_targetBoolHash, false);
            _anim.SetBool(_freeBoolHash, false);
        }

        // Same methods! 
        public void UnarmedFreeMovement(Vector2 dirVector)
        {
            _anim.SetFloat(_combatFreeForwardHash, dirVector.magnitude, _animatorDumpTime, Time.deltaTime);
        }
        public void SwordFreeMovement(Vector2 dirVector)
        {
            _anim.SetFloat(_combatFreeForwardHash, dirVector.magnitude, _animatorDumpTime, Time.deltaTime);
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
