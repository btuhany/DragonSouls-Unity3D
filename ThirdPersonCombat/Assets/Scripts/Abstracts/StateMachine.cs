using UnityEngine;
namespace States
{
    public abstract class StateMachine : MonoBehaviour
    {
        private State _currentState;
        public State PreviousState;
        public void UpdateState(float deltaTime)
        {
            if(!this.gameObject.CompareTag("Player"))
                Debug.Log(_currentState);
            _currentState?.Tick(deltaTime); //if(currentState != null) currentState.Tick(Time.deltaTime);
        }
        public void ChangeState(State newState)
        {
            State prewState = _currentState;
            _currentState?.Exit();
            _currentState = newState;
            PreviousState = prewState;
            _currentState?.Enter();
        }
    }
}
