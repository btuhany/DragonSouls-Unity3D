using UnityEngine;
using PlayerController;
using System;

namespace States
{
    public class PlayerAttackingState : PlayerBaseState
    {
        private const float _combatModeDuration = 3f;
        private AttackType attackType;
        private Attack[] _lightAttacks;
        private bool _canAttack;
        private Attack _currentAttack;

        public PlayerAttackingState(PlayerStateMachine player, AttackType attacktype ) : base(player)
        {
            _lightAttacks = combat.LightAttacks;
        }

        public override void Enter()
        {
            _currentAttack = _lightAttacks[0];
            animationController.PlayAttack(_currentAttack.animationName, _currentAttack.transitionDuration);

            inputReader.LightAttackEvent += HandleOnLightAttackEvent;
        }

        public override void Exit()
        {
            inputReader.LightAttackEvent -= HandleOnLightAttackEvent;
        }

        public override void Tick(float deltaTime)
        {
            float normalizedTime = animationController.GetAttackAnimNormalizedTime();

            if (normalizedTime >= _combatModeDuration)
            {
                stateMachine.ChangeState(stateMachine.FreeLookPlayerState);
            }
        }
        private void TryComboAttack(float normalizedTime)
        {
            if (normalizedTime < _currentAttack.comboAttackTime)
                return;

            _currentAttack = _lightAttacks[1];
            animationController.PlayAttack(_currentAttack.animationName, _currentAttack.transitionDuration);
        }
        private void HandleOnLightAttackEvent()
        {
            float normalizedTime = animationController.GetAttackAnimNormalizedTime();
            TryComboAttack(normalizedTime);
        }

    }
}
