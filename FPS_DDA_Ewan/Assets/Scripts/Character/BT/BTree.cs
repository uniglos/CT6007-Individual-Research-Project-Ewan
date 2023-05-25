using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public abstract class BTree : Character
    {
        private BTNode _root = null;

        public override void Start()
        {
            _root = SetupTree();
        }

        public override void Update()
        {
            if (_root != null)
                _root.Evaluate();
        }

        protected abstract BTNode SetupTree();
    }
}
