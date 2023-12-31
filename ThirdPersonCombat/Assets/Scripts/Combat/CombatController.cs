using System;
using UnityEngine;
using PlayerController;
using Movement;

namespace Combat
{
    [Serializable]
    public struct Attack
    {
        public string animationName;
        public float transitionDuration;
        public float attackDuration;
        public float comboPermissionDelay;
        public int damage;
        public float force;
        public float forceLerpTime;
    }
    public enum Weapon
    {
        Unarmed,
        Sword
    }
    public enum AttackType
    {
        Light,
        Heavy,
        Null
    }
    public class CombatController : MonoBehaviour
    {
        [HideInInspector] public float attackDamageBoostPercent = 0f;
        private ForceReceiver _force;
        [Header("ThrowSwordConfig")]
        [SerializeField] private Sword _sword;
        [SerializeField] private WeaponController _unarmedLeft;
        [SerializeField] private WeaponController _unarmedRight;
        [SerializeField] private float _throwForce = 200f;

        [Header("Aim")]
        [SerializeField] private GameObject _crossHairPanel;

        public bool IsSwordReturned => _sword.IsInHand || _sword.IsInSheath;
        public bool IsSwordInSheath => _sword.IsInSheath;

        public bool AttackForce = false;
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

        [HideInInspector] public Attack CurrentAttack;

        private void Awake()
        {
            _force = GetComponent<ForceReceiver>();
        }
        public void TryReturnSword()
        {
            if (IsSwordReturned) return;
            _sword.Return();
        }

        public void SetAciveCrosshair(bool enable)
        {
            if(enable)
                _crossHairPanel.SetActive(true);
            else
                _crossHairPanel.SetActive(false);
        }
        
        //AnimationEvents
        public void ThrowSword()
        {
            if (!IsSwordReturned) return;
            _sword.Throwed(Camera.main.transform.forward * _throwForce, this.transform);
        }
        public void UnsheathSword()
        {
            _sword.Unsheath();
        }

        public void SheathSword()
        {
            _sword.Sheath();
        }

        public void EnableSwordHitbox()
        {
            _sword.StartAttack(CurrentAttack.damage + (int)(CurrentAttack.damage * (attackDamageBoostPercent / 100)));
            _force.AddForce(CurrentAttack.force * transform.forward, CurrentAttack.forceLerpTime);
        }
                
        public void DisableSwordHitbox()
        {
            _sword.StopAttack();
        }
        public void EnableRightUnarmedHitboxes()
        {
            _unarmedRight.StartAttack(CurrentAttack.damage + (int)(CurrentAttack.damage * (attackDamageBoostPercent / 100)));
            _force.AddForce(CurrentAttack.force * transform.forward, CurrentAttack.forceLerpTime);
        }
        public void EnableLeftUnarmedHitbox()
        {
            _unarmedLeft.StartAttack(CurrentAttack.damage + (int)(CurrentAttack.damage * (attackDamageBoostPercent / 100)));
            _force.AddForce(CurrentAttack.force * transform.forward, CurrentAttack.forceLerpTime);
        }
        public void DisableUnarmedHitboxes()
        {
            _unarmedRight.StopAttack();
            _unarmedLeft.StopAttack();
        }
    }
}
