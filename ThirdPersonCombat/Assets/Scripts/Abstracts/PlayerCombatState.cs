using UnityEngine;
using PlayerController;

namespace States
{
    public abstract class PlayerCombatState : PlayerBaseState
    {
        protected CombatController _combat;
        private bool _autoStateChange = false;
        private Attack[] _heavyAttackArray;
        private Attack[] _lightAttackArray;
        private Attack _lightLightHeavyComboAttack;
        private int _lightAttackIndex = 0;
        private Attack _currentAttack;
        private int _heavyAttackIndex = 0;
        float _animationTimePassed = 0f;
        public PlayerCombatState(PlayerStateMachine player, Weapon weapon, bool autoStateChange = false ) : base(player)
        {
            _combat = player.CombatController;
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
            StateEnterActions();
            inputReader.LightAttackEvent += HandleOnLightAttackEvent;
            inputReader.HeavyAttackEvent += HandleOnHeavyAttackEvent;
            inputReader.TargetEvent += HandleOnTargetEvent;
        }
        public override void Tick(float deltaTime)
        {
            _animationTimePassed += deltaTime; 
           
            if (_autoStateChange)
            {
                if(_animationTimePassed > _combat.CombatModeDuration + _currentAttack.attackDuration + _currentAttack.comboPermissionDelay)
                {
                    stateMachine.ChangeState(stateMachine.FreeLookPlayerState);
                }

            }
            if (_animationTimePassed > _currentAttack.attackDuration + _currentAttack.comboPermissionDelay)
            {
                StateTickActions(deltaTime);
            }
        }
        public override void Exit()
        {
            StateExitActions();
            inputReader.LightAttackEvent -= HandleOnLightAttackEvent;
            inputReader.HeavyAttackEvent -= HandleOnHeavyAttackEvent;
            inputReader.TargetEvent -= HandleOnTargetEvent;
            _lightAttackIndex = 0;
            _heavyAttackIndex = 0;
            _animationTimePassed = 0f;
            _currentAttack = _combat.NullAttack;
        }
     
        private void TryLightComboAttack(float animationTime)
        {
            if (animationTime < _currentAttack.attackDuration)
            {
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
                    LightAttack();
                }
            }
            else
            {
                _lightAttackIndex = 0;
                LightAttack();
            }
        }

        private void TryHeavyComboAttack(float normalizedTime)
        {
            if (normalizedTime < _currentAttack.attackDuration)
            {
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
                    HeavyAttack();
                }
            }
            else
            {
                _heavyAttackIndex = 0;
                HeavyAttack();
            }
        }
        private void TryLLHComboAttack(float normalizedTime)
        {
            if (normalizedTime < _currentAttack.attackDuration)
            {
                return;
            }
            else if (normalizedTime <= _currentAttack.attackDuration + _currentAttack.comboPermissionDelay)
            {
                //Combo
                //End the combo if last attack is performed.
                _lightAttackIndex = 0;
                _currentAttack = _lightLightHeavyComboAttack;
                animationController.PlayAttack(_currentAttack.animationName, _currentAttack.transitionDuration);
                _animationTimePassed = 0f;
            }
            else
            {
                _heavyAttackIndex = 0;
                HeavyAttack();
            }
        }

        protected void LightAttack()
        {
            _heavyAttackIndex = 0;
            _currentAttack = _lightAttackArray[_lightAttackIndex];
            animationController.PlayAttack(_currentAttack.animationName, _currentAttack.transitionDuration);
            _lightAttackIndex++;
            _animationTimePassed = 0f;
        }

        protected void HeavyAttack()
        {
            _lightAttackIndex = 0;
            _currentAttack = _heavyAttackArray[_heavyAttackIndex];
            animationController.PlayAttack(_currentAttack.animationName, _currentAttack.transitionDuration);
            _heavyAttackIndex++;
            _animationTimePassed = 0f;
        }
        private void HandleOnLightAttackEvent()
        {
            TryLightComboAttack(_animationTimePassed);
        }

        private void HandleOnHeavyAttackEvent()
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
        protected virtual void HandleOnTargetEvent() { }

        protected virtual void StateEnterActions() { }
        
        protected virtual void StateExitActions() { }

        protected virtual void StateTickActions(float deltaTime) { }

    }
}
