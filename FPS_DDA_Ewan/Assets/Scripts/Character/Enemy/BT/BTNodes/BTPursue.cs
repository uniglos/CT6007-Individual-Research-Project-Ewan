using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class BTPursue : BTNode
{

    private Transform t;

    private float timer;

    public BTPursue()
    {
        t = null;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("Tried to Find Enemy");

        if (t == null)
        {
            t = (Transform)parent.GetData("target");
        }
        if(t != null)
        {
            timer += Time.deltaTime;
            EnemyBT.agent.SetDestination(t.position + ((EnemyBT.agent.transform.position - t.position).normalized * EnemyBT.agent.stoppingDistance));

            Debug.Log("Pursuing Enemy");
            if (timer > 2.0f)
            {
                EnemyBT.agent.SetDestination(t.position + ((EnemyBT.agent.transform.position - t.position).normalized * EnemyBT.agent.stoppingDistance));

                Debug.Log("Lost Enemy");
                parent.ClearData("target");
                t = null;
                EnemyBT.self.GetComponent<FieldOfView>().visibleTargets.Remove(t);
                EnemyBT.self.GetComponent<FieldOfView>().primaryTarget = null;
                state = NodeState.FAILURE;
                timer = 0;
                return state;
            }

            if (EnemyBT.self.GetComponent<FieldOfView>().visibleTargets.Contains(t))
            {
                Debug.Log("Found Enemy");
                parent.SetData("enemy", EnemyBT.self.GetComponent<FieldOfView>().primaryTarget);
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
