using UnityEngine;
namespace States
{
    public abstract class StateMachine : MonoBehaviour
    {
        private State _currentState;
        public State PreviousState;
        public void UpdateState(float deltaTime)
        {
            _currentState?.Tick(deltaTime); //if(currentState != null) currentState.Tick(Time.deltaTime);
        }
        public void ChangeState(State newState)
        {
            _currentState?.Exit();
            PreviousState = _currentState;
            _currentState = newState;
            _currentState?.Enter();
        }

    }
}
