using UnityEngine;
using States;
using EnemyControllers;
using Combat;
using UnityEngine.AI;
using Movement;

public class EnemyStateMachine : StateMachine
{
    public EnemyAnimationController AnimationController;
    public EnemyConfig Config;
    public NavMeshAgent NavmeshAgent;
    public EnemyMovementController Movement;
    public EnemyCombatController Combat;

    private Health _health;

    public EnemyIdleState enemyIdleState;
    public EnemyChaseState enemyChaseState;
    public EnemyAttackState enemyAttackState;
    public EnemyTargetState enemyTargetState;
    private void Awake()
    {
        _health = GetComponent<Health>();
        AnimationController = GetComponent<EnemyAnimationController>();
        Movement = GetComponent<EnemyMovementController>();
        NavmeshAgent = GetComponent<NavMeshAgent>();
        Combat = GetComponent<EnemyCombatController>();

        enemyIdleState = new EnemyIdleState(this);
        enemyChaseState = new EnemyChaseState(this);
        enemyAttackState = new EnemyAttackState(this);
        enemyTargetState = new EnemyTargetState(this);
    }
    private void OnEnable()
    {
        NavmeshAgent.speed = Config.MaxSpeed;
        _health.SetHealth(Config.Health);
       // ChangeState(enemyTargetState);
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
}
