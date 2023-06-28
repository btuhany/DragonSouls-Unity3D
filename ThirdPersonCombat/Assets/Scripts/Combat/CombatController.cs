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
        public float force;
        public float forceSmoothTime;
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
        [Header("ThrowSwordConfig")]
        [SerializeField] private Sword _sword;
        [SerializeField] private float _throwForce = 200f;
        [SerializeField] private float _throwTorqueMagnitude = 30f;
        private bool _isSwordThrowed = false;

        public bool AutoTarget = false;
        public float AutoTargetRotationDeltaTime = 0.15f;
        public float CombatModeDuration = 2f;
        public Attack[] UnarmedLightAttacks;
        public Attack[] UnarmedHeavyAttacks;
        public Attack UnarmedLLHComboAttack;

        public Attack[] SwordLightAttacks;
        public Attack[] SwordHeavyAttacks;
        public Attack SwordLLHComboAttack;

        public Attack ThrowAttack;

        public Attack NullAttack;

        public void ThrowSword()
        {
            _sword.Throwed(transform.forward * _throwForce, _throwTorqueMagnitude);
            _isSwordThrowed = true;
        }
        public void TryReturnSword()
        {
            if (!_isSwordThrowed) return;
            _sword.Return();
            _isSwordThrowed = false;
        }
    }
}
