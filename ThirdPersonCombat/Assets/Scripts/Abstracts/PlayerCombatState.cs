using UnityEngine;
using Combat;
using UnityEngine.UIElements;

namespace States
{
    public abstract class PlayerCombatState : PlayerBaseState
    {
        protected bool TransitionToAimState = false;
        public bool IsAttacking = false;
        protected CombatController _combat;
        private bool _autoStateChange = false;
        private Attack[] _heavyAttackArray;
        private Attack[] _lightAttackArray;
        private Attack _lightLightHeavyComboAttack;
        private int _lightAttackIndex = 0;
        private Attack _currentAttack;
        private int _heavyAttackIndex = 0;
        float _animationTimePassed = 0f;
        private AttackType _nextAttack = AttackType.Null;
        
        public PlayerCombatState(PlayerStateMachine player, Weapon weapon, bool autoStateChange = false ) : base(player)
        {
            _combat = player.combatController;
            if(weapon == Weapon.Unarmed)
            {
                _lightAttackArray = _combat.UnarmedLightAttacks;
                _heavyAttackArray = _combat.UnarmedHeavyAttacks;
                _lightLightHeavyComboAttack = _combat.UnarmedLLHComboAttack;
            }
            else if(weapon == Weapon.Sword)
            {
                _lightAttackArray = _combat.SwordLightAttacks;
                _heavyAttackArray = _combat.SwordHeavyAttacks;
                _lightLightHeavyComboAttack = _combat.SwordLLHComboAttack;
            }
            _autoStateChange = autoStateChange;
        }

        public override void Enter()
        {
            TransitionToAimState = false;
            IsAttacking = false;
            base.Enter();
        }
        public override void Tick(float deltaTime)
        {
            _animationTimePassed += deltaTime; 
           
            if (_autoStateChange)
            {
                if(_animationTimePassed > _combat.CombatModeDuration + _currentAttack.attackDuration + _currentAttack.comboPermissionDelay)
                {
                    stateMachine.ChangeState(stateMachine.freeLookPlayerState);
                }

            }

            if (_animationTimePassed > _currentAttack.attackDuration) //+ _currentAttack.comboPermissionDelay)
            {
                StateTickActions(deltaTime);
            }
            else
            {
                if(!IsAttacking)
                    IsAttacking = true;
            }

            //Auto attack from saved next attack if player pressed attack button in an attack duration.
            if (_animationTimePassed > _currentAttack.attackDuration  && _animationTimePassed < _currentAttack.attackDuration + _currentAttack.comboPermissionDelay)
            {
                if (_nextAttack != AttackType.Null)
                {
                    if (_nextAttack == AttackType.Light)
                    {
                        TryLightComboAttack(_animationTimePassed);
                    }
                    else if (_nextAttack == AttackType.Heavy)
                    {
                        //TryHeavyComboAttack(_animationTimePassed);
                        HandleOnHeavyAttackEvent(); //for checking if llh combo
                    }
                    _nextAttack = AttackType.Null;
                }
            }
        }
        public override void Exit()
        {
            _lightAttackIndex = 0;
            _heavyAttackIndex = 0;
            _animationTimePassed = 0f;
            _currentAttack = _combat.NullAttack;
            IsAttacking = false;
            base.Exit();
        }
     
        private void TryLightComboAttack(float animationTime)
        {
            if (animationTime < _currentAttack.attackDuration)
            {
                _nextAttack = AttackType.Light;
                return;
            }
            else if (animationTime <= _currentAttack.attackDuration + _currentAttack.comboPermissionDelay)
            {
                //Combo
                //End the combo if last attack is performed.
                if (_lightAttackIndex >= _lightAttackArray.Length)
                {
                    _lightAttackIndex = 0;
                }
                else
                {
                    if (stateMachine.stamina.UseStamina(movement.LightAttackStaminaCost))
                        LightAttack();
                }
            }
            else if (stateMachine.stamina.UseStamina(movement.LightAttackStaminaCost))
            {
                _lightAttackIndex = 0;
                LightAttack();
            }
        }
        private void TryHeavyComboAttack(float normalizedTime)
        {
            if (normalizedTime < _currentAttack.attackDuration)
            {
                _nextAttack = AttackType.Heavy;
                return;
            }
            else if (normalizedTime <= _currentAttack.attackDuration + _currentAttack.comboPermissionDelay)
            {
                //Combo
                //End the combo if last attack is performed.
                if (_heavyAttackIndex >= _heavyAttackArray.Length)
                {
                    _heavyAttackIndex = 0;
                }
                else
                {
                    if (stateMachine.stamina.UseStamina(movement.HeavyAttackStaminaCost))
                        HeavyAttack();
                }
            }
            else if (stateMachine.stamina.UseStamina(movement.HeavyAttackStaminaCost))
            {
                _heavyAttackIndex = 0;
                HeavyAttack();
            }
        }
        private void TryLLHComboAttack(float normalizedTime)
        {
            if (normalizedTime < _currentAttack.attackDuration)
            {
                _nextAttack = AttackType.Heavy;
                return;
            }
            else if (normalizedTime <= _currentAttack.attackDuration + _currentAttack.comboPermissionDelay && stateMachine.stamina.UseStamina(movement.HeavyAttackStaminaCost))
            {
                //Combo
                //End the combo if last attack is performed.
                _lightAttackIndex = 0;
                _currentAttack = _lightLightHeavyComboAttack;
                _combat.CurrentAttack = _currentAttack;
                animationController.PlayAttack(_currentAttack.animationName, _currentAttack.transitionDuration);
                if (_combat.AttackForce)
                {
                    forceReciver.AddForce(transform.forward * _currentAttack.force, _currentAttack.forceLerpTime);
                }
                if (_combat.AutoTarget)
                {
                    AutoTargetMovement();
                }
                _animationTimePassed = 0f;
            }
            else if (stateMachine.stamina.UseStamina(movement.HeavyAttackStaminaCost))
            {
                _heavyAttackIndex = 0;
                HeavyAttack();
            }
        }

        protected void LightAttack()
        {
            _heavyAttackIndex = 0;
            _currentAttack = _lightAttackArray[_lightAttackIndex];
            _combat.CurrentAttack = _currentAttack;
            animationController.PlayAttack(_currentAttack.animationName, _currentAttack.transitionDuration);
            _lightAttackIndex++;
            _animationTimePassed = 0f;
            if(_combat.AutoTarget)
            {
                AutoTargetMovement();
            }
            if(_combat.AttackForce)
            {
                forceReciver.AddForce(transform.forward * _currentAttack.force, _currentAttack.forceLerpTime);
            }
        }
        protected void HeavyAttack()
        {
            _lightAttackIndex = 0;
            _currentAttack = _heavyAttackArray[_heavyAttackIndex];
            _combat.CurrentAttack = _currentAttack;
            animationController.PlayAttack(_currentAttack.animationName, _currentAttack.transitionDuration);
            _heavyAttackIndex++;
            _animationTimePassed = 0f;
            if (_combat.AutoTarget)
            {
                AutoTargetMovement();
            }
            if (_combat.AttackForce)
            {
                forceReciver.AddForce(transform.forward * _currentAttack.force, _currentAttack.forceLerpTime);
            }
        }

        private void AutoTargetMovement()
        {
            if (targetableCheck.CurrentTargetTransform != null) return;
            
            Transform targetTransform = targetableCheck.GetClosestTarget().transform;

            if (targetTransform == null) return;

            RotateCharacter(movement.TargetRelativeMotionVector(targetTransform.position), _combat.AutoTargetRotationDeltaTime);

            Vector3 dir = targetTransform.position - transform.position;
            dir.y = 0f;
            
            forceReciver.AddForce(dir * 8, 0.1f);
        }
        protected override void HandleOnLightAttackEvent()
        {
            TryLightComboAttack(_animationTimePassed);
        }
        protected override void HandleOnHeavyAttackEvent()
        {
            if (_lightAttackIndex >= 2)
            {
                TryLLHComboAttack(_animationTimePassed);
            }
            else
            {
                TryHeavyComboAttack(_animationTimePassed);
            }
        }

        protected abstract void StateTickActions(float deltaTime);

        protected override void HandleOnAimHoldCancelEvent()
        {
            TransitionToAimState = false;
            base.HandleOnAimHoldCancelEvent();
        }
    }
}
