using UnityEngine;

namespace Combat
{
    public class Health : MonoBehaviour
    {
        private int _maxHealth = 100;
        private int _health;

        public int MaxHealth { get => _maxHealth; }

        public event System.Action<int,int> OnHealthUpdated;
        private void Start()
        {
            _health = _maxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (_health <= 0) return;

            _health = Mathf.Max(_health - damage, 0);
            OnHealthUpdated?.Invoke(_health, damage);
        }

        public void SetHealth(int value)
        {
            _maxHealth = value;
        }
    }
}

