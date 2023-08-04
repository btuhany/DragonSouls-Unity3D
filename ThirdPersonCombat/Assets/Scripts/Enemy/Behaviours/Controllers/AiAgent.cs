using EnemyControllers;
using Movement;
using States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiAgent : MonoBehaviour
{
    [HideInInspector] public NavMeshAgent navmeshAgent;
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public AiLocomotion locomotion;
    [HideInInspector] public Animator animator;
    [HideInInspector] public EnemyForceReceiver forceReceiver;
    [HideInInspector] public Transform playerTransform;
    private void Awake()
    {
        navmeshAgent = GetComponent<NavMeshAgent>();
        characterController = GetComponent<CharacterController>();
        locomotion = GetComponent<AiLocomotion>();
        animator = GetComponent<Animator>();
        forceReceiver = GetComponent<EnemyForceReceiver>();

        playerTransform = PlayerStateMachine.Instance.transform;
    }
    private void OnEnable()
    {
        navmeshAgent.isStopped = true;
    }
}
