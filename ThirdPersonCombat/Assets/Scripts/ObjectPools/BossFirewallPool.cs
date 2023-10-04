using UnityEngine;

public class BossFirewallPool : ObjectPoolBase<DragonFireWall>
{
    public static BossFirewallPool Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        Initialize();
    }
}
