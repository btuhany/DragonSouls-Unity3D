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
        Sword
    }
    public enum AttackType
    {
        Light,
        Heavy
    }
    public class CombatController : MonoBehaviour
    {
        public float CombatModeDuration = 2f;
        public Attack[] UnarmedLightAttacks;
        public Attack[] UnarmedHeavyAttacks;
        public Attack UnarmedLLHComboAttack;

        public Attack[] SwordLightAttacks;
        public Attack[] SwordHeavyAttacks;
        public Attack SwordLLHComboAttack;

    }
}
