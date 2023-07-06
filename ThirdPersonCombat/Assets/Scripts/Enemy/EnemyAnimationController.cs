using UnityEngine;

namespace EnemyControllers
{
    public class EnemyAnimationController : MonoBehaviour
    {
        private Animator _anim;

        private readonly int _locomotionSpeed = Animator.StringToHash("Speed");
        private readonly int _idleRunBlend = Animator.StringToHash("IdleRunBlend");
        private void Awake()
        {
            _anim = GetComponent<Animator>();
        }
        
        public void PlayIdleRunBlend(float transitionTime)
        {
            _anim.CrossFadeInFixedTime(_idleRunBlend, transitionTime);
        }
        public void SetLocomotionSpeed(float speed, float dampTime)
        {
            _anim.SetFloat(_locomotionSpeed, speed, dampTime, Time.deltaTime);
        }
        public void PlayAttack(string attackString, float transitionTime)
        {
            _anim.CrossFadeInFixedTime(attackString, transitionTime);
        }
    }
}
