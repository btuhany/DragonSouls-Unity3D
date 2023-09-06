using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetNavmeshDest : ActionNode
{
    public Vector3 direction;
    public float length;
    public float randomSphereRadius;
    public float samplePointRadius;
    protected override void OnStart()
    {
        Vector3 center = agent.transform.position;
        center = center + agent.transform.TransformDirection(direction * length);
        agent.navmeshAgent.destination = agent.RandomPointOnNavMesh(center, randomSphereRadius, samplePointRadius);
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
       return State.Success;
    }
}
