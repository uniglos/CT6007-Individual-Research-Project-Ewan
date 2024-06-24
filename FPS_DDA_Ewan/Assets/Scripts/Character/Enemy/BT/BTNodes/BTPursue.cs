using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class BTPursue : BTNode
{

    private Transform t;

    private float timer;

    private Character self;
    private NavMeshAgent agent;

    public BTPursue(Character _self, NavMeshAgent _agent)
    {
        t = null;
        agent = _agent;
        self = _self;
    }

    public override NodeState Evaluate()
    {
        //Debug.Log("Tried to Find Enemy");

        if (t == null)
        {
            t = (Transform)parent.GetData("target");
        }
        if(t != null)
        {
            timer += Time.deltaTime;
            if (Vector3.Distance(self.transform.position, t.transform.position) > 5f)
            {
                agent.SetDestination(t.position + ((agent.transform.position - t.position).normalized * agent.stoppingDistance));
            }
            //Debug.Log("Pursuing Enemy");
            if (timer > 2.0f)
            {
                agent.SetDestination(t.position + ((agent.transform.position - t.position).normalized * agent.stoppingDistance));

                //Debug.Log("Lost Enemy");
                parent.ClearData("target");
                t = null;
                self.GetComponent<FieldOfView>().visibleTargets.Remove(t);
                self.GetComponent<FieldOfView>().primaryTarget = null;
                state = NodeState.FAILURE;
                timer = 0;
                return state;
            }

            if (self.GetComponent<FieldOfView>().visibleTargets.Contains(t))
            {
                //Debug.Log("Found Enemy");
                parent.SetData("enemy", self.GetComponent<FieldOfView>().primaryTarget);
                state = NodeState.SUCCESS;
                return state;
            }
            
        }
        if (t == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        state = NodeState.FAILURE;
        return state;
    }
}
