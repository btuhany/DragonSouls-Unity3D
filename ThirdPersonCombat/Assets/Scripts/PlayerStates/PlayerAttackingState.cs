using UnityEngine;
using PlayerController;
using System;
using TMPro;

namespace States
{
    public class PlayerAttackingState : PlayerBaseState
    {
        private bool _autoStateChange = false;
        private Attack[] _heavyAttackArray;
        private Attack[] _lightAttackArray;
        private Attack _currentAttack;
        private int _lightAttackIndex = 0;
        private int _heavyAttackIndex = 0;
        private bool _layerSetted = false;
        float _animationTimePassed = 0f;
        public PlayerAttackingState(PlayerStateMachine player, Weapon weapon, bool autoStateChange = false ) : base(player)
        {
            if(weapon == Weapon.Unarmed)
            {
                _lightAttackArray = combat.UnarmedLightAttacks;
                _heavyAttackArray = combat.UnarmedHeavyAttacks;
            }
            _autoStateChange = autoStateChange;
        }

        public override void Enter()
        {
            animationController.SetLayerWeight(1, 0f);
            if (inputReader.LastAttackType == AttackType.Light)
            {
                LightAttack();
            }
            else if (inputReader.LastAttackType == AttackType.Heavy)
            {
                HeavyAttack();
            }
            inputReader.LightAttackEvent += HandleOnLightAttackEvent;
            inputReader.HeavyAttackEvent += HandleOnHeavyAttackEvent;
        }

        public override void Exit()
        {
            inputReader.LightAttackEvent -= HandleOnLightAttackEvent;
            inputReader.HeavyAttackEvent -= HandleOnHeavyAttackEvent;
            _lightAttackIndex = 0;
            _heavyAttackIndex = 0;
            _animationTimePassed = 0f;
            animationController.SetLayerWeight(1, 0f);
        }

        public override void Tick(float deltaTime)
        {
            _animationTimePassed += deltaTime; 
           
            if (_autoStateChange && _animationTimePassed > combat.CombatModeDuration + _currentAttack.attackDuration + _currentAttack.comboPermissionDelay)
            {
                stateMachine.ChangeState(stateMachine.FreeLookPlayerState);
            }
            if (_animationTimePassed > _currentAttack.attackDuration + _currentAttack.comboPermissionDelay)
            {
                if(!_layerSetted)
                {
                    animationController.SetLayerWeight(1,0.9f);
                    _layerSetted = true;
                }
                Vector2 movementOn2DAxis = inputReader.MovementOn2DAxis;
                animationController.UnarmedModeMovement(movementOn2DAxis);
                MoveCharacter(movement.CamRelativeMotionVector(movementOn2DAxis), movement.FreeLookMaxMovementSpeed, deltaTime);
                if (movementOn2DAxis.magnitude > 0f)
                {
                    RotateCharacter(movement.CamRelativeMotionVector(movementOn2DAxis), deltaTime);
                }
            }
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
                _currentAttack = combat.UnarmedLLHComboAttack;
                animationController.PlayAttack(_currentAttack.animationName, _currentAttack.transitionDuration);
            }
            else
            {
                _heavyAttackIndex = 0;
                HeavyAttack();
            }
        }
        private void HandleOnLightAttackEvent()
        {
            //float normalizedTime = animationController.GetAttackAnimNormalizedTime();
            TryLightComboAttack(_animationTimePassed);
        }

        private void HandleOnHeavyAttackEvent()
        {
            //float normalizedTime = animationController.GetAttackAnimNormalizedTime();
            if (_lightAttackIndex == 2)
            {
                TryLLHComboAttack(_animationTimePassed);
            }
            else
            {
                TryHeavyComboAttack(_animationTimePassed);
            }
        }

        private void LightAttack()
        {
            animationController.SetLayerWeight(1, 0.1f);
            _heavyAttackIndex = 0;
            _currentAttack = _lightAttackArray[_lightAttackIndex];
            animationController.PlayAttack(_currentAttack.animationName, _currentAttack.transitionDuration);
            _lightAttackIndex++;
            _animationTimePassed = 0f;
            _layerSetted = false;
        }

        private void HeavyAttack()
        {
            animationController.SetLayerWeight(1, 0.1f);
            _lightAttackIndex = 0;
            _currentAttack = _heavyAttackArray[_heavyAttackIndex];
            animationController.PlayAttack(_currentAttack.animationName, _currentAttack.transitionDuration);
            _heavyAttackIndex++;
            _animationTimePassed = 0f;
            _layerSetted = false;
        }
    }
}
