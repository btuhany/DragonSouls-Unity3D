using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class Damage : MonoBehaviour
    {
        [HideInInspector] public int AttackDamage;
        [SerializeField] Transform HitControlPosition;
        private List<Collider> _hitColliders = new List<Collider>();
        public event System.Action<Collider> OnHitGiven;

        private void OnTriggerEnter(Collider other)
        {
            if (_hitColliders.Contains(other)) return;

            if (other.TryGetComponent(out Health health))
            {
                if (_hitColliders.Contains(other)) return;
                _hitColliders.Add(other);
                health.TakeDamage(AttackDamage, this.gameObject);
                health.EnterHitPosition = HitControlPosition.position;
                if(!health.IsInvulnerable)
                    OnHitGiven?.Invoke(other);
            }
        }

        public void ResetState()
        {
            _hitColliders.Clear();
        }

        public void SetAttackDamage(int damage)
        {
            AttackDamage = damage;
        }
       
    }

}
