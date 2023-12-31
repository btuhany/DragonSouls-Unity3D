using UnityEngine;
using Combat;
using System.Collections;

namespace PlayerController
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private float _animatorDumpTime = 0.1f;
        [SerializeField] Animator _anim;

        public bool IsSprint = false;
        public bool IsUnsheathSheathAnimPlaying => _anim.GetCurrentAnimatorStateInfo(0).IsTag("Unsheath") || _anim.GetCurrentAnimatorStateInfo(0).IsTag("Sheath");
        public bool IsAttackPlaying => _anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack");


        private readonly int _swordTargetBlendTreeHash = Animator.StringToHash("SwordTargetBlendTree");
        private readonly int _unarmedTargetBlendTreeHash = Animator.StringToHash("UnarmedTargetBlendTree");
        private readonly int _unarmedFreeBlendTreeHash = Animator.StringToHash("UnarmedFreeBlendTree");
        private readonly int _swordFreeBlendTreeHash = Animator.StringToHash("SwordFreeBlendTree");
        private readonly int _freeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
        private readonly int _aimSwordBlendTreeHash = Animator.StringToHash("AimSwordBlendTree");
        private readonly int _returnSwordBlendTreeHash = Animator.StringToHash("ReturnSwordBlendTree");


        private readonly int _freeLookForwardHash = Animator.StringToHash("FreeLookForward");
        private readonly int _freeLookRightHash = Animator.StringToHash("FreeLookRight");
        private readonly int _targetforwardHash = Animator.StringToHash("TargetForward");
        private readonly int _targetrightHash = Animator.StringToHash("TargetRight");

        private readonly int _combatFreeForwardHash = Animator.StringToHash("CombatFreeForward");


        private readonly int _unsheathSwordHash = Animator.StringToHash("UnsheathSword");
        private readonly int _sheathSwordHash = Animator.StringToHash("SheathSword");
        private readonly int _isSprintHash = Animator.StringToHash("IsSprint");
        private readonly int _targetBoolHash = Animator.StringToHash("IsTarget");
        private readonly int _freeBoolHash = Animator.StringToHash("IsFree");

        private readonly int _targetedHash = Animator.StringToHash("Targeted");
        private readonly int _untargetedHash = Animator.StringToHash("Untargeted");

        private readonly int _sprintForwardHash = Animator.StringToHash("SprintForward");

        private readonly int _isRollHash = Animator.StringToHash("Roll");
        private readonly int _isFastRollHash = Animator.StringToHash("FastRoll");

        private readonly int _getHitAnim = Animator.StringToHash("PlayerGetHit");
        private readonly int _playerDeath = Animator.StringToHash("PlayerDeath");
        private readonly int _sitBonfire = Animator.StringToHash("Bonfire");
        public void PlaySetFreeLookBlend()
        {
            _anim.CrossFadeInFixedTime(_freeLookBlendTreeHash, 0.1f);
        }
        public void TargetCombat(Weapon weapon, bool play = true)
        {
            if(play)
            {
                if (weapon == Weapon.Unarmed)
                {
                    _anim.CrossFadeInFixedTime(_unarmedTargetBlendTreeHash, 0.1f);
                }
                else if (weapon == Weapon.Sword)
                {
                    _anim.CrossFadeInFixedTime(_swordTargetBlendTreeHash, 0.1f); //Virtual target camera get set by this anim.
                }
            }
            _anim.SetBool(_targetBoolHash, true);
            _anim.SetBool(_freeBoolHash, false);
        }
        public void PlayUnsheathSword()
        {
            _anim.Play(_unsheathSwordHash);
        }
        public void PlaySheathSword()
        {
            _anim.Play(_sheathSwordHash);
        }
        public void FreeCombat(Weapon weapon, bool play = true)
        {
            if(play)
            {
                if (weapon == Weapon.Unarmed)
                {
                    _anim.CrossFadeInFixedTime(_unarmedFreeBlendTreeHash, 0.1f);
                }
                else if (weapon == Weapon.Sword)
                {
                    _anim.CrossFadeInFixedTime(_swordFreeBlendTreeHash, 0.1f); //Virtual target camera get set by this anim.
                }
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

        public void TargetStateSetFloats(Vector2 dirVector)
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
        public void PlayAimSword()
        {
            _anim.CrossFadeInFixedTime(_aimSwordBlendTreeHash, 0.1f);
        }
        public void PlaySwordReturn()
        {
            _anim.CrossFadeInFixedTime(_returnSwordBlendTreeHash, 0.1f);
        }
        public void TargetedAnimation()
        {
            _anim.Play(_targetedHash);
        }
        public void UntargetedAnimation()
        {
            _anim.Play(_untargetedHash);
        }
        public void SprintSetFloats(Vector2 movementVector)
        {
            if(movementVector.sqrMagnitude > 0)
            {
                _anim.SetFloat(_sprintForwardHash, 1);
            }
            else
            {
                _anim.SetFloat(_sprintForwardHash, 0);
            }
        }

        public void PlayRoll()
        {
            _anim.CrossFadeInFixedTime(_isRollHash, 0.1f);
        }
        public void PlayFastRoll()
        {
            _anim.CrossFadeInFixedTime(_isFastRollHash, 0.1f);
        }
        public void PlayDeath()
        {
            _anim.CrossFadeInFixedTime(_playerDeath, 0.1f);
        }
        public void PlayBonfireSit()
        {
            _anim.CrossFadeInFixedTime(_sitBonfire, 0.1f);
        }
        public void PlayGetHit()
        {
            if (_isGetHitPlaying) return;
            _isGetHitPlaying = true;
            _anim.CrossFadeInFixedTime(_getHitAnim, 0.1f);
            _anim.SetLayerWeight(1, 0.5f);
            StopAllCoroutines();
            StartCoroutine(ResetWeight(1, 0.5f));
        }
        WaitForSeconds _resetTime = new WaitForSeconds(1.0f);
        bool _isGetHitPlaying = false;


        private IEnumerator ResetWeight(int index, float startValue)
        {
            yield return _resetTime;
            float elapsed = 0f;
            float duration = 1.0f;
            float value = startValue;
            while (elapsed < duration)
            {
                float t = Mathf.Clamp01(elapsed / duration);
                value = Mathf.Lerp(value, 0f, t);
                _anim.SetLayerWeight(1, value);
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }
            _isGetHitPlaying = false;
            yield return null;
        }
    }
}
