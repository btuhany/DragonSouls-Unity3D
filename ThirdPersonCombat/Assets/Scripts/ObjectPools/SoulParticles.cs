using UnityEngine;

public class SoulParticles : MonoBehaviour
{
    public ParticleSystem particleFx;
    private void Awake()
    {
        particleFx= GetComponent<ParticleSystem>();
    }
}
