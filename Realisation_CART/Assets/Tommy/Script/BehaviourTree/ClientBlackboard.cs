using CartControl;
using DiscountDelirium;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviourTree
{
    [System.Serializable]
    public class ClientBlackboard
    {
        public string m_name;
        public GameObject m_thisClient;
		public CartStateMachine m_cartStateMachine;
        public NavMeshAgent m_navAgent;
        public List<GameObject> m_chosenPathListCopy;
        public ClientPathList m_possiblePathScript;

        public List<Vector3> m_path;
        public List<GameObject> m_pathDebugBox;
        public Vector3 m_target;
        public float m_targetAngle;

        public float m_stuckAtTime;
        public float m_timeStuck;

	}
}
