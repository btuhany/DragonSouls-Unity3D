using UnityEngine;
namespace States
{
    public abstract class StateMachine : MonoBehaviour
    {
        protected State _currentState;
        public State PreviousState;
        public void UpdateState(float deltaTime)
        {
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
