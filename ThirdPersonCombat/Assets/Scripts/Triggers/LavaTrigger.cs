using Combat;
using States;
using UnityEngine;

public class LavaTrigger : MonoBehaviour
{
    [SerializeField] private int _attackDamage = 999;
    Damage _damage;
    private void Awake()
    {
        _damage = GetComponent<Damage>();
        _damage.SetAttackDamage(_attackDamage);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Health health))
        {
            _damage.GiveDamageForced(health, other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        _damage.ResetState();
    }
}
