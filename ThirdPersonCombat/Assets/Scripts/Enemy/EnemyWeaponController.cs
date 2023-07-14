using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    Collider _collider;
    private void Awake()
    {
        _collider = GetComponent<Collider>();  
    }

    public void EnableWeapon()
    {
        _collider.enabled = true;
    }
    public void DisableWeapon()
    {
        _collider.enabled = false;
    }
}
