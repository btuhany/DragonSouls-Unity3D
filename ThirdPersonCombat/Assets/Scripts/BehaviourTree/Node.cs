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

    [HideInInspector] public State mState = State.Running;
    [HideInInspector] public bool Started = false;
    [HideInInspector] public string Guid;
    [HideInInspector] public Vector2 Position;
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

    public virtual Node Clone()
    {
        return Instantiate(this);
    }
    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract State OnUpdate();
}
