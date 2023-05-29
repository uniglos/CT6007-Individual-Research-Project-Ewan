using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//This Behaviour Tree is similar to one I used in my CT6024 Advanced AI Algorithms
//All behaviour Tree setup code was
//Adapted from Mina Pêcheux's video on YouTube
namespace BehaviourTree
{

    public abstract class BTree : Character
    {
        private BTNode _root = null;

        //public Character self;
        //public NavMeshAgent agent;

        public override void Start()
        {
            base.Start();
            _root = SetupTree();
        }

        public override void Update()
        {
            base.Update();
            if (!isDead)
            {
                if (_root != null)
                    _root.Evaluate();
            }
        }

        protected abstract BTNode SetupTree();
    }
}
