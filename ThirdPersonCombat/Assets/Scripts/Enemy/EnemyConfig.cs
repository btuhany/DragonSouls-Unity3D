using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    public int Health;

    [Header("Movement")]
    public float MaxSpeed = 5f;

    [Header("States")]
    public EnemyBaseState InitialState;

    [Header("Animations")]
    public float IdleRunAnimTransitionTime = 0.1f;
    public float AnimationDampTime = 0.1f;

    [Header("Detection")]
    public float MinIdleRange = 20f;

    [Header("ChaseState")]
    public float DestinationPointRefreshTime = 0.5f;
    public float MaxAttackDistance = 0.5f;

    [Header("AttackState")]
    public float LookRotationLerpTimeMultiplier = 0.2f;
    public float MinWaitTime = 0.5f;
    public float MaxWaitTime = 2f;

    [Header("TargetState")]
    public float TargetedMovementSpeed = 2f;
}
