using BoxSystem;
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
		[Header("Client Stats")]
		[Range(1, 10)] public float m_wantMostActiveShelves;
		[Range(1, 10)] public float m_wantClosestPath;
		[Range(5, 30)] public float m_sightRange;
		[Range(15, 180)] public float m_sightHalfAngle;
		[Range(1, 100)] public float m_aggressiveness;
		[Range(1, 10)] public float m_tacticalAttack;
		public float m_pursuitTenacity;

		[Header("Don't touch")]
		public string m_name;
        public GameObject m_thisClient;
		public CartStateMachine m_cartStateMachine;
        public NavMeshAgent m_navAgent;
        public List<GameObject> m_chosenPathListCopy;   //This is a list of premade points that set a path along shelf
        public ClientPathList m_possiblePathScript;
        public TowerBoxSystem m_thisTower;
        public GameObject m_cashRegister;
        public List<Vector3> m_path;        //This is to store the path points generated by the navmesh
        public List<GameObject> m_pathDebugBox;
        public Vector3 m_target;
        public float m_targetAngle;
        public float m_currentPursuitStartTime;
        public float m_currentSpeed;    // meter/seconds

        public bool m_isGameStarted = false;

		public float m_stuckAtTime;
        public float m_timeStuck;
        public float m_lastAttackTimer;
		public List<GameObject> m_clientInSight;
        public bool m_isAttacking;
        public int m_chosenTarget;
      
	}
}
