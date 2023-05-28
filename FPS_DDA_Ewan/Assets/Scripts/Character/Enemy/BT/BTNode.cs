using System.Collections;
using System.Collections.Generic;

namespace BehaviourTree
{
    using UnityEngine.AI;
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }
    public class BTNode
    {
        protected NodeState state;
        public BTNode parent;
        protected List<BTNode> children = new List<BTNode>();

        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        public BTNode()
        {
            parent = null;
        }
        public BTNode(List<BTNode> children)
        {
            foreach (BTNode child in children)
                _Attach(child);
        }

        private void _Attach(BTNode node)
        {
            node.parent = this;
            children.Add(node);
        }

        public virtual NodeState Evaluate() => NodeState.FAILURE;

        //Shared Data between nodes
        //If any data needs to be passed from node to node
        //A dictionary is set up to store these values

        //Each key in the dictionary is a string
        public void SetData(string key, object value)
        {
            if (_dataContext.ContainsKey(key))
            {
                _dataContext[key] = value;
            }
            else
            {
                _dataContext.Add(key, value);
                
            }
        }

        //Recursively searches for the key to output data
        //if it reaches the root node, there is no dictionary entry
        public object GetData(string key)
        {
            object value = null;
            if (_dataContext.TryGetValue(key, out value))
                return value;

            BTNode node = parent;
            while(node != null)
            {
                value = node.GetData(key);
                if (value != null)
                    return value;
                node = node.parent;
            }
            return null;
        }

        //Recursively searches for the key to clear the data
        //if it reaches the root node, there is no dictionary entry
        public bool ClearData(string key)
        {
            if (_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }

            BTNode node = parent;
            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                    return true;
                node = node.parent;
            }
            return false;
        }
    }
}
