using UnityEngine;

namespace Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int _maxHealth = 100;
        private int _health;

        private void Start()
        {
            _health = _maxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (_health <= 0) return;

            _health = Mathf.Max(_health - damage, 0);
        }
    }
}

