using UnityEngine;

public class MageSmallProjectilePool : ObjectPoolBase<ProjectileController>
{
    public static MageSmallProjectilePool Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        Initialize();
    }
}
