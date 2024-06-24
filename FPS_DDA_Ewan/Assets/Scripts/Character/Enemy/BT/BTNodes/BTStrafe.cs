using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;
public class BTStrafe : BTNode
{
    public float wanderRadius;
    public float wanderTimer;

    private Transform target;

    private NavMeshAgent agent;

    private float timer;

    private bool hasStrafed = false;

    public BTStrafe(float _wanderRadius, float _wanderTimer, NavMeshAgent _agent)
    {
        wanderRadius = _wanderRadius;
        wanderTimer = 3.0f;
        agent = _agent;
    }
    public override NodeState Evaluate()
    {
        timer += Time.deltaTime;
        if (!hasStrafed)
        {
            Vector3 newPos = RandomNavSphere(agent.transform.position, wanderRadius, -1);
            Debug.Log("Strafing");
            agent.SetDestination(newPos);
            hasStrafed = true;
        }
        

        if (timer >= wanderTimer)
        {
            timer = 0;
            hasStrafed = false; 
            Debug.Log("Strafe Timer Finished");
            state = NodeState.SUCCESS;
            return state;
        }
        state = NodeState.RUNNING;
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
