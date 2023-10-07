using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulParticlesPool : ObjectPoolBase<SoulParticles>
{
    public static SoulParticlesPool Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        Initialize();
    }
}
