using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;
public class BTWander : BTNode
{
    public float wanderRadius;
    public float wanderTimer;

    private Transform target;

    private NavMeshAgent agent;
    
    private float timer;

    public BTWander(float _wanderRadius, float _wanderTimer, NavMeshAgent _agent)
    {
        wanderRadius = _wanderRadius;
        wanderTimer = _wanderTimer;
        agent = _agent;
    }
    public override NodeState Evaluate()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(agent.transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }

        state = NodeState.SUCCESS;
        return state;
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}