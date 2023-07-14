using UnityEngine;
using States;
using EnemyControllers;
using Combat;
using UnityEngine.AI;
using System.Collections;

public class EnemyStateMachine : StateMachine
{
    public EnemyAnimationController AnimationController;
    public EnemyConfig Config;
    public NavMeshAgent NavmeshAgent;
    public EnemyMovementController Movement;
    public EnemyCombatController Combat;

    private Health _health;

    public EnemyIdleState IdleState;
    public EnemyChaseState ChaseState;
    public EnemyAttackState AttackState;
    public EnemyTargetState TargetState;

    private bool _isInAttackConditionCheck = false;
    private bool _isInChaseConditionCheck = false;
    private void Awake()
    {
        _health = GetComponent<Health>();
        AnimationController = GetComponent<EnemyAnimationController>();
        Movement = GetComponent<EnemyMovementController>();
        NavmeshAgent = GetComponent<NavMeshAgent>();
        Combat = GetComponent<EnemyCombatController>();

        IdleState = new EnemyIdleState(this);
        ChaseState = new EnemyChaseState(this);
        AttackState = new EnemyAttackState(this);
        TargetState = new EnemyTargetState(this);
    }
    private void OnEnable()
    {
        NavmeshAgent.speed = Config.MaxSpeed;
        _health.SetHealth(Config.Health);
        ChangeState(TargetState);
    }
    private void Update()
    {
        UpdateState(Time.deltaTime);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Config.MinIdleRange);
    }

    public bool IsPlayerInRange(float range)
    {
        if (Vector3.Distance(this.transform.position, PlayerStateMachine.Instance.transform.position) < range)
            return true;
        else
            return false;
    }
    
    public void StartAttackStateCheck(WaitForSeconds waitTime, float range)
    {
        if (_isInAttackConditionCheck) return;
        _isInAttackConditionCheck = true;
        StartCoroutine(CheckAttackStateConditions(waitTime, range));
    }
    public void StartChaseStateCheck(WaitForSeconds waitTime, float range)
    {
        if (_isInChaseConditionCheck) return;
        _isInChaseConditionCheck = true;
        StartCoroutine(CheckChaseStateConditions(waitTime, range));
    }

    private IEnumerator CheckAttackStateConditions(WaitForSeconds waitTime, float range)
    {
        yield return waitTime;
        if (IsPlayerInRange(range))
            ChangeState(AttackState);
        _isInAttackConditionCheck = false;
        yield return null;
    }
    private IEnumerator CheckChaseStateConditions(WaitForSeconds waitTime, float range)
    {
        yield return waitTime;
        if (IsPlayerInRange(range))
            ChangeState(ChaseState);
        _isInChaseConditionCheck = false;
        yield return null;
    }
}
