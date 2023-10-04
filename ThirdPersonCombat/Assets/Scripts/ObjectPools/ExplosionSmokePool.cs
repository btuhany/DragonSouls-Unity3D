using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSmokePool : ObjectPoolBase<ExplosionSmoke>
{
    public static ExplosionSmokePool Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        Initialize();
    }
}
