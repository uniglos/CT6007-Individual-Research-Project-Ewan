using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This Behaviour Tree is similar to one I used in my CT6024 Advanced AI Algorithms
//All behaviour Tree setup code was
//Adapted from Mina Pêcheux's video on YouTube
namespace BehaviourTree
{
    public abstract class BTree : Character
    {
        private BTNode _root = null;

        public override void Start()
        {
            _root = SetupTree();
            Debug.Log("Ran");
        }

        public override void Update()
        {
            if (_root != null)
                _root.Evaluate();
        }

        protected abstract BTNode SetupTree();
    }
}
