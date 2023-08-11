using Combat;
using EnemyControllers;
using States;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AiAgent : MonoBehaviour
{
    [HideInInspector] public NavMeshAgent navmeshAgent;
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public AiLocomotion locomotion;
    [HideInInspector] public Animator animator;
    [HideInInspector] public EnemyForceReceiver forceReceiver;
    [HideInInspector] public EnemyCombatController combat;
    [HideInInspector] public Health health;

    [HideInInspector] public Transform playerTransform;

    private BehaviourTreeRunner _treeRunner;
    private void Awake()
    {
        navmeshAgent = GetComponent<NavMeshAgent>();
        characterController = GetComponent<CharacterController>();
        locomotion = GetComponent<AiLocomotion>();
        animator = GetComponent<Animator>();
        forceReceiver = GetComponent<EnemyForceReceiver>();
        combat = GetComponent<EnemyCombatController>();
        health = GetComponent<Health>();
        _treeRunner = GetComponent<BehaviourTreeRunner>();

        playerTransform = PlayerStateMachine.Instance.transform;
    }

    private void OnEnable()
    {
        navmeshAgent.isStopped = true;
        health.OnHealthUpdated += HandleOnTakeHit;
    }
    private void OnDisable()
    {
        health.OnHealthUpdated -= HandleOnTakeHit;
    }
    private void HandleOnTakeHit(int health, int damage)
    {
        _treeRunner.Tree.blackboard.isHit = true;
        StopAllCoroutines();
        StartCoroutine(ResetIsHit());
    }
    WaitForSeconds _resetIsHitTime = new WaitForSeconds(0.1f);
    private IEnumerator ResetIsHit()
    {
        yield return _resetIsHitTime;
        _treeRunner.Tree.blackboard.isHit = false;
        yield return null;
    }
}
