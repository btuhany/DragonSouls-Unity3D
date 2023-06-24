using UnityEngine;
namespace States
{
    public abstract class StateMachine : MonoBehaviour
    {
        private State _currentState;

        public void UpdateState(float deltaTime)
        {
            _currentState?.Tick(deltaTime); //if(currentState != null) currentState.Tick(Time.deltaTime);
        }
        public void ChangeState(State newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState?.Enter();
        }

    }
}
