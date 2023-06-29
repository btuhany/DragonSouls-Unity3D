using Combat;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public int AttackDamage;

    private List<Collider> _hitColliders = new List<Collider>();
    private void OnEnable()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(_hitColliders.Contains(other)) return;
        
        if(other.TryGetComponent(out Health health))
        {
            _hitColliders.Add(other);
            health.TakeDamage(AttackDamage);
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
    public void OnDisable()
    {
        
    }
}
