using UnityEngine;

public class BossFireProjectilePool : ObjectPoolBase<DragonFireProjectile>
{
    public static BossFireProjectilePool Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        Initialize();
    }
}
