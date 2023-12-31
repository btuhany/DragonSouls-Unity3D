using Combat;
using EnemyControllers;
using PlayerController;
using States;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine
{
    public int soulPoint = 10;
    [SerializeField] private bool _debug = false;
    public EnemyAnimationController animController;
    public EnemyConfig config;
    public NavMeshAgent navmeshAgent;
    public EnemyMovementController movementController;
    public EnemyCombatController combatController;
    public EnemyForceReceiver forceReceiver;
    public SkeletonSoundController sound;

    public EnemyIdleState idleState;
    public EnemyChaseState chaseState;
    public EnemyAttackState attackState;
    public EnemyTargetState targetState;
    public EnemyDeadState deadState;
    public EnemyGetHitState getHitState;
    public EnemySwordPierced swordHitState;

    public Health health;

    public event System.Action OnDead; 

    private Targetable _targetController;

    private bool _isInAttackConditionCheck = false;
    private bool _isInChaseConditionCheck = false;
    [HideInInspector] public bool isSwordOn = false;
    [HideInInspector] public Sword sword;
    [HideInInspector] public bool isDead = false;
    private Vector3 _initialWorldPos;
    private Quaternion _initialWorldRotation;
    [HideInInspector] public bool isOnDeadState = false;
    private void Awake()
    {
        _initialWorldPos = transform.position;
        _initialWorldRotation = transform.rotation;

        _targetController = GetComponent<Targetable>();
        health = GetComponent<Health>();
        animController = GetComponent<EnemyAnimationController>();
        movementController = GetComponent<EnemyMovementController>();
        combatController = GetComponent<EnemyCombatController>();
        forceReceiver = GetComponent<EnemyForceReceiver>();

        idleState = new EnemyIdleState(this);
        chaseState = new EnemyChaseState(this);
        attackState = new EnemyAttackState(this);
        targetState = new EnemyTargetState(this);
        deadState = new EnemyDeadState(this);
        getHitState = new EnemyGetHitState(this);
        swordHitState = new EnemySwordPierced(this);
        BonfiresManager.Instance.OnTakeRestEvent += HandleOnPlayerTookRest;
        PlayerStateMachine.Instance.OnPlayerRespawn += HandleOnPlayerTookRest;
    }
    private void OnEnable()
    {
        navmeshAgent.isStopped = true;
        isDead = false;
        isOnDeadState = false;
        health.OnHealthUpdated += HandleOnHealthUpdated;
        health.SetMaxHealth(config.Health);
        OnDead += forceReceiver.HandleOnDead;
        ChangeState(idleState);
    }
    private void OnDisable()
    {
        forceReceiver.isCharacterControllerDisabled = false;
        health.OnHealthUpdated -= HandleOnHealthUpdated;
        OnDead -= forceReceiver.HandleOnDead;
    }
    private void Update()
    {
        if (isDead) { return; }
        UpdateState(Time.deltaTime);

        if (_debug)
            Debug.Log(_currentState);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, config.MinIdleRange);
    }

    public bool IsPlayerInRange(float range)
    {
        if (Vector3.Distance(this.transform.position, PlayerStateMachine.Instance.transform.position) < range && !PlayerStateMachine.Instance.isInvisible)
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
            isOnDeadState = true;
            OnDead?.Invoke();
            _targetController.ResetTargetable();
            ChangeState(deadState);
            return;
        }
        else
        {
            if (_currentState == getHitState)
                getHitState.GetHitAgain();
            else if (_currentState == swordHitState)
                swordHitState.OnGetHit();
            else
                ChangeState(getHitState);
        }
    }
    private IEnumerator CheckAttackStateConditions(WaitForSeconds waitTime, float range)
    {
        yield return waitTime;
        if (IsPlayerInRange(range))
            ChangeState(attackState);
        _isInAttackConditionCheck = false;
        yield return null;
    }
    private IEnumerator CheckChaseStateConditions(WaitForSeconds waitTime, float range)
    {
        yield return waitTime;
        if (IsPlayerInRange(range))
            ChangeState(chaseState);
        _isInChaseConditionCheck = false;
        yield return null;
    }
    private void HandleOnPlayerTookRest()
    {
        isDead = false;
        isOnDeadState = false;
        health.ResetHealth();
        if(!movementController.CharacterController.enabled)
            movementController.CharacterController.enabled = true;
        this.gameObject.SetActive(false);
        transform.position = _initialWorldPos;
        transform.rotation = _initialWorldRotation;
        this.gameObject.SetActive(true);
    }
}
