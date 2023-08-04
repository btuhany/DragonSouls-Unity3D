using UnityEngine;
using States;
using EnemyControllers;
using Combat;
using UnityEngine.AI;
using System.Collections;
using PlayerController;

public class EnemyStateMachine : StateMachine
{
    [SerializeField] private bool _debug = false;
    public EnemyAnimationController AnimationController;
    public EnemyConfig Config;
    public NavMeshAgent NavmeshAgent;
    public EnemyMovementController Movement;
    public EnemyCombatController Combat;
    public EnemyForceReceiver EnemyForceReceiver;

    public EnemyIdleState IdleState;
    public EnemyChaseState ChaseState;
    public EnemyAttackState AttackState;
    public EnemyTargetState TargetState;
    public EnemyDeadState DeadState;
    public EnemyGetHitState GetHitState;
    public EnemySwordPierced SwordHitState;

    public Health Health;

    public event System.Action OnDead; 

    private Targetable _targetController;

    private bool _isInAttackConditionCheck = false;
    private bool _isInChaseConditionCheck = false;
    public bool IsSwordOn = false;
    public Sword Sword;
    public bool IsDead = false;
    private void Awake()
    {
        _targetController = GetComponent<Targetable>();
        Health = GetComponent<Health>();
        AnimationController = GetComponent<EnemyAnimationController>();
        Movement = GetComponent<EnemyMovementController>();
        Combat = GetComponent<EnemyCombatController>();
        EnemyForceReceiver = GetComponent<EnemyForceReceiver>();

        IdleState = new EnemyIdleState(this);
        ChaseState = new EnemyChaseState(this);
        AttackState = new EnemyAttackState(this);
        TargetState = new EnemyTargetState(this);
        DeadState = new EnemyDeadState(this);
        GetHitState = new EnemyGetHitState(this);
        SwordHitState = new EnemySwordPierced(this);

        NavmeshAgent.isStopped = true;
    }
    private void OnEnable()
    {
        Health.OnHealthUpdated += HandleOnHealthUpdated;
        Health.SetHealth(Config.Health);
        ChangeState(ChaseState);
    }
    private void Update()
    {
        if (IsDead) { return; }
        UpdateState(Time.deltaTime);

        if (_debug)
            Debug.Log(_currentState);
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

    private void HandleOnHealthUpdated(int health, int damage)
    {
        if (health <= 0)
        {
            ChangeState(DeadState);
            OnDead?.Invoke();
            _targetController.ResetTargetable();
        }
        else
        {
            if (_currentState == GetHitState)
                GetHitState.GetHitAgain();
            else
                ChangeState(GetHitState);
        }
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
