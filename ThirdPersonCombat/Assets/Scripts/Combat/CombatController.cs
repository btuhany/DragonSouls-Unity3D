using System;
using UnityEngine;

namespace PlayerController
{
    [Serializable]
    public struct Attack
    {
        public string animationName;
        public float transitionDuration;
        public float attackDuration;
        public float comboPermissionDelay;
      
    }
    public enum Weapon
    {
        Unarmed,
    }
    public enum AttackType
    {
        Light,
        Heavy
    }
    public class CombatController : MonoBehaviour
    {
        [SerializeField] PlayerAnimationController _animationController;
        public float CombatModeDuration = 2f;
        public Attack[] UnarmedLightAttacks;
        public Attack[] UnarmedHeavyAttacks;
        public Attack UnarmedLLHComboAttack;
        public void PlayUnarmedAttack(int index)
        {
            Attack attack = UnarmedLightAttacks[index];
            _animationController.PlayAttack(attack.animationName, attack.transitionDuration);
        }
    }
}
