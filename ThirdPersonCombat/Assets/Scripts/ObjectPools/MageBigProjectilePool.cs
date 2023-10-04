using UnityEngine;

public class MageBigProjectilePool : ObjectPoolBase<ProjectileController>
{
    public static MageBigProjectilePool Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        Initialize();
    }
}
