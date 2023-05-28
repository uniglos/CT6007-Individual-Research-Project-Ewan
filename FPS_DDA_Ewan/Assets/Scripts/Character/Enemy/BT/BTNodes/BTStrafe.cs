using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class BTStrafe : BTNode
{
    public float wanderRadius;
    public float wanderTimer;

    private Transform target;

    private float timer;


    public override NodeState Evaluate()
    {

        state = NodeState.RUNNING;
        return state;
    }
}
