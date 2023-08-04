using DG.Tweening;
using UnityEngine;

public class AiLocomotion : MonoBehaviour
{
    public void LookRotation(Vector3 dir, float lerpTimeScale, float deltaTime)
    {
        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.LookRotation(dir),
                deltaTime * lerpTimeScale
                );
        }
    }
}
