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
    public float MinIdleRange = 30f;

    [Header("ChaseState")]
    public float ChasePointRefreshTime = 0.5f;
    public float ChaseToTargetChangeMaxRange = 9f;
    public float ChaseToTargetChangeMinRange = 2f;
    public float ChaseToAttackChangeRange = 1f;
    public float ChaseToTargetChangeProbability = 0.25f;
    public float ChaseReactionMinWaitTime = 0f;
    public float ChaseReactionMaxWaitTime = 1.2f;
    public float ChaseToIdleChangeRange = 20f;

    [Header("AttackState")]
    public float LookRotationLerpTimeMultiplier = 0.2f;
    public float AttackMinWaitTime = 0.5f;
    public float AttackMaxWaitTime = 2f;
    public float AttackToTargetChangeRange = 2f;
    public float AttackToChaseChangeRange = 10f;
    public float AttackReactionMinWaitTime = 0f;
    public float AttackReactionMaxWaitTime = 1.2f;

    [Header("TargetState")]
    public float TargetMovementSpeed = 1f;
    public float TargetMaxRightDirChangeTime = 6f;
    public float TargetMinRightDirChangeTime = 1f;
    public float TargetMinBackDirChangeTime = 3f;
    public float TargetMaxBackDirChangeTime = 0.5f;
    public float TargetMinApproachPlayerTime = 5f;
    public float TargetMaxApproachPlayerTime = 10f;
    public float TargetMinSpeedUpTime = 0.8f;
    public float TargetMaxSpeedUpTime = 2f;
    public float TargetMinSpeedUpValue = 1.5f;
    public float TargetMaxSpeedUpValue = 3f;
    public float TargetBackDirChangeProbility = 0.2f;
    public float TargetToAttackChangeRange = 1f;
    public float TargetToChaseChangeRange = 10f;

    [Header("GetHitState")]
    public float GetHitAnimationTime = 0.7f;
}
