using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

public class EnemyBT : BTree
{
    public static NavMeshAgent agent;

    private void Awake()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();
    }
    protected override BTNode SetupTree()
    {
        BTNode root = new BTWander(30, 3);
        return root;
    }
}
