using DiscountDelirium;
using UnityEngine;

namespace BehaviourTree
{
    //Class to put on a GameObject to run the BehaviourTree
    public class BehaviourTreeRunner : MonoBehaviour
    {
        public BehaviourTreeObject m_tree;

		void Start()
        {
            m_tree.m_blackboard.m_thisClient = gameObject;
            //To allow multiple instance of the same BehaviourTree to execute at the same time
            m_tree = m_tree.Clone();

            m_tree.m_blackboard.m_name = gameObject.name;
            //Check to integrate some AiAgent here
            m_tree.Bind();

            m_tree.m_blackboard.m_possiblePathScript = ClientPathList._ClientPathList;

            if (m_tree.m_blackboard.m_possiblePathScript == null) Debug.LogError("m_possiblePathScript not found");
        }

        void Update()
        {
            m_tree.Update();
        }

	}
}
