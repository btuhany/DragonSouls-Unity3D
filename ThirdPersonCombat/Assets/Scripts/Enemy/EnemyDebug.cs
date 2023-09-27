using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class EnemyDebug : MonoBehaviour
{
    [SerializeField] float _range;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
}
