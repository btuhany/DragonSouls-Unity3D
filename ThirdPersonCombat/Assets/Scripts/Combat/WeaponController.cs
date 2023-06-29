using UnityEngine;

namespace Combat
{
    [RequireComponent(typeof(Damage))]
    public class WeaponController : MonoBehaviour
    {
        private Damage _damage;
        private CapsuleCollider _collider;

        private void Awake()
        {
            _damage = GetComponent<Damage>();
            _collider = GetComponent<CapsuleCollider>();
        }

        public void StartAttack(int damage)
        {
            _collider.enabled = true;
            _damage.ResetState();
            _damage.SetAttackDamage(damage);
            _damage.enabled = true;
        }

        public void StopAttack()
        {
            _collider.enabled = false;
            _damage.enabled = false;
        }

    }
}

