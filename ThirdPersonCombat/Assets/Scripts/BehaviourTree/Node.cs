using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : ScriptableObject
{
    public enum State
    {
        Running,
        Failure,
        Success
    }

    public State mState = State.Running;
    public bool Started = false;
    public State Update()
    {
        if (!Started)
        {
            OnStart();
            Started = true;
        }

        mState = OnUpdate();

        if (mState == State.Failure || mState == State.Success)
        {
            OnStop();
            Started = false;
        }

        return mState;
    }

    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract State OnUpdate();
}
