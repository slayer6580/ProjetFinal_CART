using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
	public class RandomizeStats : LeafNode
	{
		public int m_minActive;
		public int m_maxActive;
		public int m_minClosest;
		public int m_maxClosest;
		public int m_minAggressive;
		public int m_maxAggressive;
		public int m_minTacticalAtk;
		public int m_maxTacticalAtk;

		protected override void OnStart()
		{
			
		}

		protected override void OnStop()
		{
	
		}

		protected override State OnUpdate()
		{
			m_blackboard.m_wantMostActiveShelves = Random.Range(m_minActive, m_maxActive);
			m_blackboard.m_wantClosestPath = Random.Range(m_minClosest, m_maxClosest);
			m_blackboard.m_aggressiveness = Random.Range(m_minAggressive, m_maxAggressive);
			m_blackboard.m_tacticalAttack = Random.Range(m_minTacticalAtk, m_maxTacticalAtk);

			return State.Success;
		}
	}
}
