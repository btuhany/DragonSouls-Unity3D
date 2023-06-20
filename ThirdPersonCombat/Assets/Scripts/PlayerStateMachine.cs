using System;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [SerializeField] private InputReader inputReader;

    private void OnEnable()
    {
        inputReader.JumpEvent += HandleOnJump;
        inputReader.DodgeEvent += HandleOnDodge;
    }

    private void HandleOnDodge()
    {
        Debug.Log("Dodged!");
    }

    private void HandleOnJump()
    {
        Debug.Log("Jumped!");
    }
}
