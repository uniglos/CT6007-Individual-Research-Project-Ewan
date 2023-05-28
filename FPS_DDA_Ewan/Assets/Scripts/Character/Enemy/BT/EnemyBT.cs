using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

//This Behaviour Tree is similar to one I used in my CT6024 Advanced AI Algorithms
//Adapted from Mina Pêcheux's video on YouTube
public class EnemyBT : BTree
{
    public Character self;
    public NavMeshAgent agent;

    private void Awake()
    {
        
        self = this.gameObject.GetComponent<Character>();
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        agent.stoppingDistance = 3;
    }

    public override void Update()
    {
        base.Update();
        if(self.moveSpeed != agent.speed)
        {
            agent.speed = self.moveSpeed;
        }
    }
    protected override BTNode SetupTree()
    {
        BTNode root =  new Selector(new List<BTNode>
        {
            new Sequence(new List<BTNode> 
            {
                //Pursue enemy if you have one
                new BTHaveTarget(self),
                new BTPursue(self,agent),
                new BTShoot(self),
                /*new BTStrafe()*/
            }),
            new BTWander(30, 3.0f,agent)
        }); 
        //Shoot player
        //Strafe Player

        //Wander until you find player

        return root;
    }
}
