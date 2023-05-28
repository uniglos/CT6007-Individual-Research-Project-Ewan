using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class BTHaveTarget : BTNode
{
    private List<Transform> visibleTargets;

    public BTHaveTarget()
    {
        
    }

    public override NodeState Evaluate()
    {
        if(EnemyBT.self.GetComponent<FieldOfView>().visibleTargets.Count >0)
        {
            state = NodeState.SUCCESS;
            Debug.Log("Have Target");
            parent.SetData("target", EnemyBT.self.GetComponent<FieldOfView>().visibleTargets[0]);
            EnemyBT.self.GetComponent<FieldOfView>().primaryTarget = 
                EnemyBT.self.GetComponent<FieldOfView>().visibleTargets[0].gameObject.GetComponent<Character>();
        }
        else
        {
            state = NodeState.FAILURE;
        }

        return state;
    }
}
