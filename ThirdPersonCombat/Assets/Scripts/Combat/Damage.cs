using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class Damage : MonoBehaviour
    {
        [HideInInspector] public int AttackDamage;
        [SerializeField] Transform HitControlPosition;
        private List<Collider> _hitColliders = new List<Collider>();
     
        private void OnTriggerEnter(Collider other)
        {
            if (_hitColliders.Contains(other)) return;

            if (other.TryGetComponent(out Health health))
            {
                if (_hitColliders.Contains(other)) return;
                _hitColliders.Add(other);
                health.TakeDamage(AttackDamage);
                health.EnterHitPosition = HitControlPosition.position;
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
