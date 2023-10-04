using UnityEngine;

public class BossFireballPool : ObjectPoolBase<DragonFireball>
{
    public static BossFireballPool Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        Initialize();
    }
}
