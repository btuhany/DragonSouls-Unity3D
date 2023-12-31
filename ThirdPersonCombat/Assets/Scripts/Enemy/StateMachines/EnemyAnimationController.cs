using UnityEngine;

namespace EnemyControllers
{
    public class EnemyAnimationController : MonoBehaviour
    {
        private Animator _anim;

        public bool IsGetHitPlaying => _anim.GetCurrentAnimatorStateInfo(0).IsTag("GetHit");

        private readonly int _locomotionSpeed = Animator.StringToHash("Speed");
        private readonly int _idleRunBlend = Animator.StringToHash("IdleRunBlend");
        private readonly int _targetedBlendTree = Animator.StringToHash("TargetedBlendTree");
        private readonly int _getHitCenter = Animator.StringToHash("GetHitCenter");
        private readonly int _getHitRight = Animator.StringToHash("GetHitRight");
        private readonly int _getHitLeft = Animator.StringToHash("GetHitLeft");
        private readonly int _dead = Animator.StringToHash("Dead");
        private readonly int _isTargeted = Animator.StringToHash("IsTargeted");
        private readonly int _targetedRight = Animator.StringToHash("TargetRight");
        private readonly int _targetedForward = Animator.StringToHash("TargetForward");
        private void Awake()
        {
            _anim = GetComponent<Animator>();
        }
        
        public void PlayIdleRunBlend(float transitionTime)
        {
            if (_anim == null) _anim = GetComponent<Animator>();
            _anim.CrossFadeInFixedTime(_idleRunBlend, transitionTime);
            _anim.SetBool(_isTargeted, false);
        }
        public void PlayTargetedBlend(float transitionTime)
        {
            _anim.CrossFadeInFixedTime(_targetedBlendTree, transitionTime);
            _anim.SetBool(_isTargeted, true);
        }

        public void SetIdleRunLocomotionSpeed(float speed, float dampTime)
        {
            if (_anim == null) _anim = GetComponent<Animator>();
             _anim.SetFloat(_locomotionSpeed, speed, dampTime, Time.deltaTime);
        }
        public void SetTargetedBool(bool targeted)
        {
            _anim.SetBool(_isTargeted, targeted);
        }
        public void SetTargetedLocomotionSpeed(float rightSpeed, float forwardSpeed, float dampTime)
        {
            _anim.SetFloat(_targetedRight, rightSpeed, dampTime, Time.deltaTime);
            _anim.SetFloat(_targetedForward, forwardSpeed, dampTime, Time.deltaTime);
        }
        public void PlayAttack(string attackString, float transitionTime)
        {
            _anim.CrossFadeInFixedTime(attackString, transitionTime);
        }
        public void PlayDeadAnimation()
        {
            _anim.CrossFadeInFixedTime(_dead, 0.1f);
        }
        public void PlayGetHitAnimation(int leftright)
        {
            if (leftright == 0)
                _anim.CrossFadeInFixedTime(_getHitCenter, 0.1f);
            else if (leftright == -1)
                _anim.CrossFadeInFixedTime(_getHitCenter, 0.1f);
            else if (leftright == 1)
                _anim.CrossFadeInFixedTime(_getHitCenter, 0.1f);
        }
        public void PauseAnimation()
        {
            _anim.enabled= false;
        }
        public void ResumeAnimation()
        {
            _anim.enabled= true;
        }
    }
}
