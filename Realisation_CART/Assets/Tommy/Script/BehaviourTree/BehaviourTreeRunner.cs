using UnityEngine;

namespace BehaviourTree
{
    //Class to put on a GameObject to run the BehaviourTree
    public class BehaviourTreeRunner : MonoBehaviour
    {
        public BehaviourTree m_tree;
     
        void Start()
        {
            //To allow multiple instance of the same BehaviourTree to execute at the same time
            m_tree = m_tree.Clone();
        }

        void Update()
        {
            m_tree.Update();
        }
    }
}
