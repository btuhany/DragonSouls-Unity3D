using UnityEngine;

namespace Combat
{
    public class Health : MonoBehaviour
    {
        private int _armorPercent = 0;
        public bool IsInvulnerable = false;
        public int maxHealth = 100;
        private int _health;
        public Damage lastDamageObj;
        [HideInInspector] public Vector3 EnterHitPosition;
        public bool IsDead { get; private set; }
        public event System.Action<int,int> OnHealthUpdated;
        public event System.Action OnHealthIncreased;
        public event System.Action OnDead;
        public event System.Action OnArmorUpgrade;
        private void Start()
        {
            _health = maxHealth;
        }

        public virtual void TakeDamage(int damage, Damage damageObj)
        {
            if (IsInvulnerable || IsDead) return;
            if (_health <= 0) return;
            damage = damage - (damage * _armorPercent) / 100;
            _health = Mathf.Max(_health - damage, 0);
            if(_health <= 0)
            {
                IsDead = true;
                OnDead?.Invoke();
            }
            lastDamageObj  = damageObj;
            OnHealthUpdated?.Invoke(_health, damage);
        }

        public void SetMaxHealth(int value)
        {
            maxHealth = value;
        }
        public void IncreaseHealth(int value)
        {
            if(IsDead) return;
            if (_health >= maxHealth) return;
            _health = Mathf.Min(_health + value, maxHealth);
            OnHealthUpdated?.Invoke(_health, 0);
            OnHealthIncreased?.Invoke();
        }
        public void IncreaseMaxHealth(int value)
        {
            maxHealth += value;
            _health = maxHealth;
        }
        public void ResetHealth()
        {
            IsDead = false;
            _health = maxHealth;
            OnHealthUpdated?.Invoke(maxHealth, 0);
        }
        public void IncreaseArmor(int value)
        {
            _armorPercent += value;
            OnArmorUpgrade?.Invoke();
        }
    }
}

