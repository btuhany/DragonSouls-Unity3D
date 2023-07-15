using UnityEngine;

namespace Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int _maxHealth = 100;
        private int _health;
        public Vector3 LastHitPosition;
        public int MaxHealth { get => _maxHealth; }

        public event System.Action<int,int> OnHealthUpdated;
        private void Start()
        {
            _health = _maxHealth;
        }

        public void TakeDamage(int damage, Vector3 damagePos)
        {
            if (_health <= 0) return;
            LastHitPosition = damagePos;
            _health = Mathf.Max(_health - damage, 0);
            OnHealthUpdated?.Invoke(_health, damage);
        }

        public void SetHealth(int value)
        {
            _maxHealth = value;
        }
    }
}

