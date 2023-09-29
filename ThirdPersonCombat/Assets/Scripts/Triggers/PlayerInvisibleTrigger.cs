using States;
using UnityEngine;

public class PlayerInvisibleTrigger : MonoBehaviour
{
    [SerializeField] private bool _isInvisible;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            PlayerStateMachine.Instance.isInvisible = _isInvisible;
    }
}
