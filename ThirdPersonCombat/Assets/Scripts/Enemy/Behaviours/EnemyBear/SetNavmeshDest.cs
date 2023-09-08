using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SetNavmeshDest : ActionNode
{
    public Vector3 direction;
    public float lenght;
    public float randomSphereRadius;
    public float samplePointRadius;
    public bool randomLenght = false;
    public float minLenght;
    public float maxLenght;
    protected override void OnStart()
    {
        if(randomLenght)
        {
            lenght = Random.Range(minLenght, maxLenght);
        }
        Vector3 center = agent.transform.position;
        center = center + agent.transform.TransformDirection(direction * lenght);
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
