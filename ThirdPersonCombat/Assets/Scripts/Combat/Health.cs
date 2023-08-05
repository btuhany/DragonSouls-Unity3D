using UnityEngine;

namespace Combat
{
    public class Health : MonoBehaviour
    {
        public bool IsInvulnerable = false;
        public int maxHealth = 100;
        private int _health;
        [HideInInspector] public Vector3 EnterHitPosition;

        public event System.Action<int,int> OnHealthUpdated;
        private void Start()
        {
            _health = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (IsInvulnerable) return;
            if (_health <= 0) return;
            _health = Mathf.Max(_health - damage, 0);
            OnHealthUpdated?.Invoke(_health, damage);
        }

        public void SetHealth(int value)
        {
            maxHealth = value;
        }
    }
}

