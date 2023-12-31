using UnityEngine;
using States;
using EnemyControllers;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.AI;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine stateMachine;
    protected EnemyAnimationController animationController;
    protected EnemyConfig config;
    protected NavMeshAgent navmeshAgent;
    protected EnemyMovementController movement;
    public EnemyBaseState(EnemyStateMachine enemy) 
    {
        this.stateMachine = enemy;
        this.animationController = enemy.animController;
        this.config = enemy.config;
        this.navmeshAgent = enemy.navmeshAgent;
        this.movement = enemy.movementController;
    }

}
