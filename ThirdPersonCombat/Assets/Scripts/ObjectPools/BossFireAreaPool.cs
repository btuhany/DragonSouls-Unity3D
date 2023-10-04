using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFireAreaPool : ObjectPoolBase<DragonFireArea>
{
    public static BossFireAreaPool Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        Initialize();
    }
}
