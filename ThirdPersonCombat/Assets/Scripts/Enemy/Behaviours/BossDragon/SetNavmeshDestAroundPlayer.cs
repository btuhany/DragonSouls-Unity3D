using States;
using UnityEngine;

public class SetNavmeshDestAroundPlayer : ActionNode
{
    public float radius;
    public float randomSphereRadius;
    public float samplePointRadius;
    public bool isRandomRadius;
    public float minRadius;
    public float maxRadius;

    protected override void OnStart()
    {
        if(isRandomRadius)
        {
            radius = Random.Range(minRadius, maxRadius);
        }
        Vector2 dir = RandomPointOnUnitCircle() * radius;
        Vector3 destination = PlayerStateMachine.Instance.transform.position + new Vector3(dir.x, 0, dir.y);
        agent.navmeshAgent.destination = agent.RandomPointOnNavMesh(destination, randomSphereRadius, samplePointRadius);
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return State.Success;
    }
    private Vector2 RandomPointOnUnitCircle()
    {
        float angle = Random.Range(0, 360f);
        float x = Mathf.Cos(angle);
        float y = Mathf.Sin(angle);
        return new Vector2(x, y);
    }
}
