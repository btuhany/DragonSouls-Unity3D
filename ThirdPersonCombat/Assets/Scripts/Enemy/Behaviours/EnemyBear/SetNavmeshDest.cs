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
    public bool randomDir;
    public Vector3 minDirValues;
    public Vector3 maxDirValues;
    protected override void OnStart()
    {
        if(randomLenght)
        {
            lenght = Random.Range(minLenght, maxLenght);
        }
        if(randomDir)
        {
            direction = new Vector3(
                Random.Range(minDirValues.x, maxDirValues.x),
                Random.Range(minDirValues.y, maxDirValues.y),
                Random.Range(minDirValues.z, maxDirValues.z)
                );
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
