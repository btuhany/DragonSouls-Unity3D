using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;

public class LimbHealth : Health
{
    private const float invulnurableTime = 0.2f;
    [SerializeField] private Health mainHealth;
    public override void TakeDamage(int damage, Damage damageObj)
    {
        mainHealth.TakeDamage(damage, damageObj);
        mainHealth.IsInvulnerable = true;
        StopAllCoroutines();
        StartCoroutine(StartTakeDamageCooldown());
    }
    WaitForSeconds cooldown = new WaitForSeconds(invulnurableTime);
    IEnumerator StartTakeDamageCooldown()
    {
        yield return cooldown;
        mainHealth.IsInvulnerable = false;
        yield return null;
    }
}
