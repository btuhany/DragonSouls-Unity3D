using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    private State currentState;
    private float timeOnCurrentState;
    private void Update()
    {
        timeOnCurrentState += Time.deltaTime;
        currentState?.Tick(Time.deltaTime); //if(currentState != null) currentState.Tick(Time.deltaTime);
    }
    public void ChangeState(State newState)
    {
        timeOnCurrentState = 0f;
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

}
