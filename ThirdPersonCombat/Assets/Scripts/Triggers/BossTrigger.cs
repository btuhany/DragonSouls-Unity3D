using UnityEngine;
using States;

public class BossTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (BossManager.Instance.IsInBoss) return;
        if (other.TryGetComponent(out PlayerStateMachine player))
        {
            BossManager.Instance.StartBossFight();
        }
    }
}
