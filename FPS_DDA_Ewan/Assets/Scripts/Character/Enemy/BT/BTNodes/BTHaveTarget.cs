using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class BTHaveTarget : BTNode
{
    private List<Transform> visibleTargets;

    private Character self;

    public BTHaveTarget(Character _self)
    {
        self = _self;
    }

    public override NodeState Evaluate()
    {
        if(self.GetComponent<FieldOfView>().visibleTargets.Count >0)
        {
            state = NodeState.SUCCESS;
            Debug.Log("Have Target");
            parent.SetData("target", self.GetComponent<FieldOfView>().visibleTargets[0]);
            self.GetComponent<FieldOfView>().primaryTarget = 
                self.GetComponent<FieldOfView>().visibleTargets[0].gameObject.GetComponent<Character>();
        }
        else
        {
            state = NodeState.FAILURE;
        }

        return state;
    }
}
