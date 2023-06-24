using System;
using UnityEngine;

namespace PlayerController
{
    [Serializable]
    public struct Attack
    {
        public string animationName;
        public float transitionDuration;
        public float comboAttackTime;
    }
    public enum AttackType
    {
        Light,
        Heavy
    }
    public class CombatController : MonoBehaviour
    {
        public Attack[] LightAttacks;
    }
}
