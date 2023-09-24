using Combat;
using UnityEngine;

public class StartAttack : ActionNode
{
    [Tooltip("For attack array in enemy combat controller")] public int attackNum;
    public bool isRandomAttack;
    public int minRandomIndex = 0;
    public int randomLength;
    EnemyCombatController combat;
    protected override void OnStart()
    {
        if (combat == null)
        {
            combat = agent.combat;
        }

        blackboard.attackTimeCounter = 0f;
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        Attack attack;

        if (isRandomAttack)
            attack = combat.Attacks[Random.Range(minRandomIndex, randomLength)];
        else
            attack = combat.Attacks[attackNum];

        combat.CurrentAttack = attack;
        agent.animator.CrossFadeInFixedTime(attack.animationName, attack.transitionDuration);

        if(attack.force > 1)
        {
            agent.forceReceiver.AddForce();
        }

        blackboard.attackTimeCounter += Time.deltaTime;

        return State.Success;
    }
}
