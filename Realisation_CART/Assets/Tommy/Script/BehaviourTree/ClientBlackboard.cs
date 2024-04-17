using CartControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    [System.Serializable]
    public class ClientBlackboard
    {
        public string m_name;
        public GameObject m_thisClient;
		public CartStateMachine m_cartStateMachine;

        public List<Vector3> m_path;
        public Vector3 m_target;
    }
}
