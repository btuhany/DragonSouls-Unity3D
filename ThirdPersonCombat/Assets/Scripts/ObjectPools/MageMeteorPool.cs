using UnityEngine;

public class MageMeteorPool : ObjectPoolBase<MageMeteorParticleController>
{
    public static MageMeteorPool Instance { get; private set; }
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        Initialize();
    }
}
